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
            modelBuilder.Entity<TransportEntry>()
            .Property(e => e.Method)
            .HasConversion<int>(); 
            modelBuilder.Entity<Users>().ToTable("users").HasKey(x => x.UserId);
            modelBuilder.Entity<Challenge>().ToTable("challenges").HasKey(x => x.ChallengeId);
            modelBuilder.Entity<UserChallengeProgress>().ToTable("userChallengeProgress").HasKey(x => x.UserChallengeProgressId);
            modelBuilder.Entity<Company>().ToTable("companies").HasKey(x => x.CompanyId);
            modelBuilder.Entity<Teams>().ToTable("teams").HasKey(x => x.TeamId);
            modelBuilder.Entity<Achievement>().ToTable("achievement").HasKey(x => x.AchievementId);
            modelBuilder.Entity<Achievement>().Property(a => a.ConditionType).HasConversion<string>();
            modelBuilder.Entity<UserAchievement>().ToTable("userachievement").HasKey(x => x.UserAchievementId);
            modelBuilder.Entity<EndUserAddress>().ToTable("enduseraddress").HasKey(x => x.EndUserAddressId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<API> API { get; set; }
        public DbSet<TransportEntry> TransportEntry { get; set; }
        public DbSet<Challenge> Challenge { get; set; }
        public DbSet<UserChallengeProgress> UserChallengeProgress { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<Achievement> Achievement { get; set; }
        public DbSet<UserAchievement> UserAchievement { get; set; }
        public DbSet<EndUserAddress> EndUserAddress { get; set; }
    }
}