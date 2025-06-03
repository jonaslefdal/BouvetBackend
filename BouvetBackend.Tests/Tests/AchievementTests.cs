using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BouvetBackend.Entities;
using BouvetBackend.Repositories;
using BouvetBackend.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BouvetBackend.Tests.Repositories
{
    public class AchievementRepositoryTests
    {
        private class TestEfAchievementRepository : EfAchievementRepository
        {
            public List<UserAchievement> Awarded { get; } = new();

            public TestEfAchievementRepository(DataContext context) : base(context) { }

            protected override async Task AwardAchievement(int userId, Achievement achievement)
            {
                Awarded.Add(new UserAchievement
                {
                    UserId = userId,
                    AchievementId = achievement.AchievementId,
                    EarnedAt = DateTime.UtcNow
                });

                await Task.CompletedTask;
            }
        }

        // test wether or not a user actually gets an achivement when they meet threshold
        [Fact]
        public async Task CheckForAchievements_AwardsAchievement_WhenThresholdMet()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("AchievementTestDb")
                .Options;

            using var context = new DataContext(options);

            int userId = 1;

            // Setup dummy achievement for walking 10 km
            context.Achievement.Add(new Achievement
            {
                AchievementId = 1,
                ConditionType = AchievementCondition.DistanceWalking,
                Threshold = 10,
                Description = "Gå 10 km"
            });

            // Empty list = user has not yet earned any
            context.UserAchievement.RemoveRange(context.UserAchievement.ToList());

            await context.SaveChangesAsync();

            // Fake entries to pass threshold
            var entries = new List<TransportEntry>
            {
                new() { UserId = userId, Method = Methode.Walking, DistanceKm = 6 },
                new() { UserId = userId, Method = Methode.Walking, DistanceKm = 5 }
            };

            var userChallenges = new List<UserChallengeProgress>();

            var repo = new TestEfAchievementRepository(context);

            // Act
            await repo.CheckForAchievements(userId, Methode.Walking, entries, userChallenges);

            // Assert
            Assert.Single(repo.Awarded);
            Assert.Equal(1, repo.Awarded[0].AchievementId);
            Assert.Equal(userId, repo.Awarded[0].UserId);
        }

        // Make sure a user dont get the same achivement multiple times
        [Fact]
        public async Task CheckForAchievements_DoesNotAward_WhenAlreadyEarned()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase("AchievementAlreadyTestDb")
                .Options;

            using var context = new DataContext(options);
            int userId = 2;

            var achievement = new Achievement
            {
                AchievementId = 2,
                ConditionType = AchievementCondition.TotalEntries,
                Threshold = 2,
                Description = "Registrer 2 reiser"
            };

            context.Achievement.Add(achievement);
            context.UserAchievement.Add(new UserAchievement
            {
                UserId = userId,
                AchievementId = achievement.AchievementId,
                EarnedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();

            var entries = new List<TransportEntry>
    {
        new() { UserId = userId, Method = Methode.Bus, DistanceKm = 1 },
        new() { UserId = userId, Method = Methode.Bus, DistanceKm = 1 },
        new() { UserId = userId, Method = Methode.Bus, DistanceKm = 1 }
    };

            var userChallenges = new List<UserChallengeProgress>();

            var repo = new TestEfAchievementRepository(context);

            // Act
            await repo.CheckForAchievements(userId, Methode.Bus, entries, userChallenges);

            // Assert
            Assert.Empty(repo.Awarded);
        }

        // check the progress and make sure correct calculations
        [Theory]
        [InlineData(AchievementCondition.DistanceWalking, Methode.Walking, 12)]
        [InlineData(AchievementCondition.DistanceCycling, Methode.Cycling, 12)]
        [InlineData(AchievementCondition.TotalEntries, Methode.Bus, 3)]
        [InlineData(AchievementCondition.DistanceCar, Methode.Car, 12)]
        public async Task GetAchievementProgress_ReturnsCorrectValue_ForVariousConditions(
        AchievementCondition condition, Methode method, int expectedProgress)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase($"ProgressDb_{condition}_{method}")
                .Options;

            using var context = new DataContext(options);
            int userId = 99;

            context.Achievement.Add(new Achievement
            {
                AchievementId = 123,
                ConditionType = condition,
                Threshold = 100, // irrelevant, vi sjekker bare progresjon
                Description = "Test achievement"
            });

            await context.SaveChangesAsync();

            var entries = new List<TransportEntry>
    {
        new() { UserId = userId, Method = method, DistanceKm = 6 },
        new() { UserId = userId, Method = method, DistanceKm = 6 },
        new() { UserId = userId, Method = method, DistanceKm = 0 } // for entry count
    };

            var userChallenges = new List<UserChallengeProgress>();

            var repo = new EfAchievementRepository(context);

            // Act
            var result = await repo.GetAchievementProgress(userId, entries, userChallenges);

            // Assert
            Assert.True(result.TryGetValue(123, out var progress));
            Assert.Equal(expectedProgress, progress);
        }
    }
}
