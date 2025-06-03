using Xunit;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BouvetBackend.Controllers;
using BouvetBackend.Entities;
using BouvetBackend.Repositories;
using BouvetBackend.Models.CompleteChallengeModel;


namespace BouvetBackend.Tests.Controllers
{
    public class ChallengeControllerTests
    {
        private static ChallengeController CreateControllerWithUser(
            string email,
            out Mock<IChallengeRepository> challengeRepo,
            out Mock<IUserRepository> userRepo,
            out Mock<IUserChallengeProgressRepository> progressRepo,
            out Mock<ITransportEntryRepository> transportRepo,
            out Mock<IAchievementRepository> achievementRepo)
        {
            challengeRepo = new Mock<IChallengeRepository>();
            progressRepo = new Mock<IUserChallengeProgressRepository>();
            userRepo = new Mock<IUserRepository>();
            transportRepo = new Mock<ITransportEntryRepository>();
            achievementRepo = new Mock<IAchievementRepository>();

            var controller = new ChallengeController(
                challengeRepo.Object,
                progressRepo.Object,
                userRepo.Object,
                transportRepo.Object,
                achievementRepo.Object,
                serviceProvider: null!
            );

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("emails", email)
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }

        [Fact]
        public void GetUserChallenges_ReturnsGroupedAttempts()
        {
            // Arrange
            var AzureId = "123";
            var email = "test@bouvet.no";
            var userId = 1;

            var controller = CreateControllerWithUser(email,
                out var challengeRepo,
                out var userRepo,
                out var progressRepo,
                out var transportRepo,
                out var achievementRepo);

            userRepo.Setup(r => r.GetUserByEmail(email)).Returns(new Users { AzureId = AzureId, UserId = userId, Email = email });

            progressRepo.Setup(r => r.GetAttemptsByUserId(userId)).Returns(new List<UserChallengeProgress>
            {
                new() { ChallengeId = 10, UserId = userId },
                new() { ChallengeId = 10, UserId = userId },
                new() { ChallengeId = 11, UserId = userId }
            });

            // Act
            var result = controller.GetUserChallenges() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var list = result.Value as IEnumerable<object>;
            Assert.NotNull(list);
            Assert.Contains(list, o => o.ToString()!.Contains("ChallengeId = 10"));
            Assert.Contains(list, o => o.ToString()!.Contains("ChallengeId = 11"));
        }

        [Fact]
        public void CompleteChallenge_ReturnsOk_WhenValid()
        {
            // Arrange
            var AzureId = "123";
            var email = "test@bouvet.no";
            var userId = 2;
            var challengeId = 5;

            var controller = CreateControllerWithUser(email,
                out var challengeRepo,
                out var userRepo,
                out var progressRepo,
                out var transportRepo,
                out var achievementRepo);

            var user = new Users { AzureId = AzureId, UserId = userId, Email = email };
            var challenge = new Challenge
            {
                ChallengeId = challengeId,
                Points = 10,
                MaxAttempts = 2
            };

            userRepo.Setup(r => r.GetUserByEmail(email)).Returns(user);
            challengeRepo.Setup(r => r.Get(challengeId)).Returns(challenge);
            progressRepo.Setup(r => r.GetAttemptsByUserForChallenge(userId, challengeId))
                .Returns(new List<UserChallengeProgress> { new() { AttemptedAt = DateTime.UtcNow } });

            var request = new CompleteChallengeRequest { ChallengeId = challengeId };

            // Act
            var result = controller.CompleteChallenge(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Value?.ToString()?.Contains("Challenge fully completed") ?? false);
        }

        [Fact]
        public void CompleteChallenge_ReturnsBadRequest_WhenTooManyAttempts()
        {
            // Arrange
            var AzureId = "123";
            var email = "test@bouvet.no";
            var userId = 3;
            var challengeId = 9;

            var controller = CreateControllerWithUser(email,
                out var challengeRepo,
                out var userRepo,
                out var progressRepo,
                out var transportRepo,
                out var achievementRepo);

            var user = new Users { AzureId = AzureId, UserId = userId, Email = email };
            var challenge = new Challenge
            {
                ChallengeId = challengeId,
                Points = 10,
                MaxAttempts = 2
            };

            userRepo.Setup(r => r.GetUserByEmail(email)).Returns(user);
            challengeRepo.Setup(r => r.Get(challengeId)).Returns(challenge);
            progressRepo.Setup(r => r.GetAttemptsByUserForChallenge(userId, challengeId))
                .Returns(new List<UserChallengeProgress>
                {
                    new() { AttemptedAt = DateTime.UtcNow },
                    new() { AttemptedAt = DateTime.UtcNow }
                });

            var request = new CompleteChallengeRequest { ChallengeId = challengeId };

            // Act
            var result = controller.CompleteChallenge(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
