﻿// <auto-generated />
using System;
using BouvetBackend.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BouvetBackend.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250402035212_NullHandling")]
    partial class NullHandling
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BouvetBackend.Entities.API", b =>
                {
                    b.Property<int>("apiId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("apiId"));

                    b.Property<int>("value1")
                        .HasColumnType("integer");

                    b.Property<int>("value2")
                        .HasColumnType("integer");

                    b.HasKey("apiId");

                    b.ToTable("api", (string)null);
                });

            modelBuilder.Entity("BouvetBackend.Entities.Achievement", b =>
                {
                    b.Property<int>("AchievementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AchievementId"));

                    b.Property<string>("ConditionType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Threshold")
                        .HasColumnType("integer");

                    b.HasKey("AchievementId");

                    b.ToTable("achievement", (string)null);
                });

            modelBuilder.Entity("BouvetBackend.Entities.Challenge", b =>
                {
                    b.Property<int>("ChallengeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ChallengeId"));

                    b.Property<string>("ConditionType")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("MaxAttempts")
                        .HasColumnType("integer");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<double?>("RequiredDistanceKm")
                        .HasColumnType("double precision");

                    b.Property<int>("RequiredTransportMethod")
                        .HasColumnType("integer");

                    b.Property<int>("RotationGroup")
                        .HasColumnType("integer");

                    b.HasKey("ChallengeId");

                    b.ToTable("challenges", (string)null);
                });

            modelBuilder.Entity("BouvetBackend.Entities.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CompanyId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("CompanyId");

                    b.ToTable("companies", (string)null);
                });

            modelBuilder.Entity("BouvetBackend.Entities.Teams", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TeamId"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<int>("MaxMembers")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("TeamId");

                    b.HasIndex("CompanyId");

                    b.ToTable("teams", (string)null);
                });

            modelBuilder.Entity("BouvetBackend.Entities.TransportEntry", b =>
                {
                    b.Property<int>("TransportEntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransportEntryId"));

                    b.Property<double>("Co2")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("DistanceKm")
                        .HasColumnType("double precision");

                    b.Property<int>("Method")
                        .HasColumnType("integer");

                    b.Property<double>("MoneySaved")
                        .HasColumnType("double precision");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("TransportEntryId");

                    b.HasIndex("UserId");

                    b.ToTable("transportEntries", (string)null);
                });

            modelBuilder.Entity("BouvetBackend.Entities.UserAchievement", b =>
                {
                    b.Property<int>("UserAchievementId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserAchievementId"));

                    b.Property<int>("AchievementId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EarnedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("UserAchievementId");

                    b.HasIndex("AchievementId");

                    b.HasIndex("UserId");

                    b.ToTable("userachievement", (string)null);
                });

            modelBuilder.Entity("BouvetBackend.Entities.UserChallengeProgress", b =>
                {
                    b.Property<int>("UserChallengeProgressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserChallengeProgressId"));

                    b.Property<DateTime>("AttemptedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ChallengeId")
                        .HasColumnType("integer");

                    b.Property<int>("PointsAwarded")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("UserChallengeProgressId");

                    b.HasIndex("ChallengeId");

                    b.HasIndex("UserId");

                    b.ToTable("userChallengeProgress", (string)null);
                });

            modelBuilder.Entity("BouvetBackend.Entities.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<string>("AzureId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("NickName")
                        .HasColumnType("text");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("text");

                    b.Property<int?>("TeamId")
                        .HasColumnType("integer");

                    b.Property<int>("TotalScore")
                        .HasColumnType("integer");

                    b.HasKey("UserId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("TeamId");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("BouvetBackend.Entities.Teams", b =>
                {
                    b.HasOne("BouvetBackend.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("BouvetBackend.Entities.TransportEntry", b =>
                {
                    b.HasOne("BouvetBackend.Entities.Users", "Users")
                        .WithMany("TransportEntry")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("BouvetBackend.Entities.UserAchievement", b =>
                {
                    b.HasOne("BouvetBackend.Entities.Achievement", "Achievement")
                        .WithMany()
                        .HasForeignKey("AchievementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BouvetBackend.Entities.Users", "Users")
                        .WithMany("UserAchievements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Achievement");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("BouvetBackend.Entities.UserChallengeProgress", b =>
                {
                    b.HasOne("BouvetBackend.Entities.Challenge", "Challenge")
                        .WithMany()
                        .HasForeignKey("ChallengeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BouvetBackend.Entities.Users", "Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Challenge");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("BouvetBackend.Entities.Users", b =>
                {
                    b.HasOne("BouvetBackend.Entities.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId");

                    b.HasOne("BouvetBackend.Entities.Teams", "Team")
                        .WithMany("Users")
                        .HasForeignKey("TeamId");

                    b.Navigation("Company");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("BouvetBackend.Entities.Company", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("BouvetBackend.Entities.Teams", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("BouvetBackend.Entities.Users", b =>
                {
                    b.Navigation("TransportEntry");

                    b.Navigation("UserAchievements");
                });
#pragma warning restore 612, 618
        }
    }
}
