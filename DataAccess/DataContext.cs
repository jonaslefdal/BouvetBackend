using BouvetBackend.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BouvetBackend.DataAccess
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<API>().ToTable("api").HasKey(x => x.apiId);
            modelBuilder.Entity<TransportEntry>().ToTable("transportEntries").HasKey(x => x.TransportEntryId);
            modelBuilder.Entity<Users>().ToTable("users").HasKey(x => x.UserId);
            modelBuilder.Entity<Challenge>().ToTable("challenges").HasKey(x => x.ChallengeId);
            modelBuilder.Entity<UserChallengeAttempt>().ToTable("userChallengeAttempts").HasKey(x => x.UserChallengeAttemptId);
            modelBuilder.Entity<Company>().ToTable("companies").HasKey(x => x.CompanyId);
            modelBuilder.Entity<Teams>().ToTable("teams").HasKey(x => x.TeamId);
            modelBuilder.Entity<Achievement>().ToTable("achievement").HasKey(x => x.AchievementId);
            modelBuilder.Entity<UserAchievement>().ToTable("userachievement").HasKey(x => x.UserAchievementId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<API> API { get; set; }
        public DbSet<TransportEntry> TransportEntry { get; set; }
        public DbSet<Challenge> Challenge { get; set; }
        public DbSet<UserChallengeAttempt> UserChallengeAttempt { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<Achievement> Achievement { get; set; }
        public DbSet<UserAchievement> UserAchievement { get; set; }

    }
}