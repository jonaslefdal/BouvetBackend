﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BouvetBackend.Migrations
{
    /// <inheritdoc />
    public partial class tableUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "achievement",
                columns: table => new
                {
                    AchievementId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ConditionType = table.Column<string>(type: "text", nullable: false),
                    Threshold = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_achievement", x => x.AchievementId);
                });

            migrationBuilder.CreateTable(
                name: "api",
                columns: table => new
                {
                    apiId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    value1 = table.Column<int>(type: "integer", nullable: false),
                    value2 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api", x => x.apiId);
                });

            migrationBuilder.CreateTable(
                name: "challenges",
                columns: table => new
                {
                    ChallengeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    MaxAttempts = table.Column<int>(type: "integer", nullable: false),
                    RotationGroup = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_challenges", x => x.ChallengeId);
                });

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "weeklychallenge",
                columns: table => new
                {
                    WeeklyChallengeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WeekStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ChallengeId = table.Column<int>(type: "integer", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weeklychallenge", x => x.WeeklyChallengeId);
                    table.ForeignKey(
                        name: "FK_weeklychallenge_challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "challenges",
                        principalColumn: "ChallengeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teams", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_teams_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AzureId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: true),
                    TotalScore = table.Column<int>(type: "integer", nullable: false),
                    NickName = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    TeamId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_users_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_users_teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "teams",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateTable(
                name: "transportEntries",
                columns: table => new
                {
                    TransportEntryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Method = table.Column<string>(type: "text", nullable: false),
                    Co2 = table.Column<double>(type: "double precision", nullable: false),
                    DistanceKm = table.Column<double>(type: "double precision", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transportEntries", x => x.TransportEntryId);
                    table.ForeignKey(
                        name: "FK_transportEntries_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userachievement",
                columns: table => new
                {
                    UserAchievementId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    AchievementId = table.Column<int>(type: "integer", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userachievement", x => x.UserAchievementId);
                    table.ForeignKey(
                        name: "FK_userachievement_achievement_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "achievement",
                        principalColumn: "AchievementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userachievement_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "userChallengeAttempts",
                columns: table => new
                {
                    UserChallengeAttemptId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ChallengeId = table.Column<int>(type: "integer", nullable: false),
                    PointsAwarded = table.Column<int>(type: "integer", nullable: false),
                    AttemptedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userChallengeAttempts", x => x.UserChallengeAttemptId);
                    table.ForeignKey(
                        name: "FK_userChallengeAttempts_challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "challenges",
                        principalColumn: "ChallengeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userChallengeAttempts_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_teams_CompanyId",
                table: "teams",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_transportEntries_UserId",
                table: "transportEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_userachievement_AchievementId",
                table: "userachievement",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_userachievement_UserId",
                table: "userachievement",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_userChallengeAttempts_ChallengeId",
                table: "userChallengeAttempts",
                column: "ChallengeId");

            migrationBuilder.CreateIndex(
                name: "IX_userChallengeAttempts_UserId",
                table: "userChallengeAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_users_CompanyId",
                table: "users",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_users_TeamId",
                table: "users",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_weeklychallenge_ChallengeId",
                table: "weeklychallenge",
                column: "ChallengeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "api");

            migrationBuilder.DropTable(
                name: "transportEntries");

            migrationBuilder.DropTable(
                name: "userachievement");

            migrationBuilder.DropTable(
                name: "userChallengeAttempts");

            migrationBuilder.DropTable(
                name: "weeklychallenge");

            migrationBuilder.DropTable(
                name: "achievement");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "challenges");

            migrationBuilder.DropTable(
                name: "teams");

            migrationBuilder.DropTable(
                name: "companies");
        }
    }
}
