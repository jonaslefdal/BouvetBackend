﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BouvetBackend.Migrations
{
    /// <inheritdoc />
    public partial class renameAndChangesAchive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "achievement",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "achievement");
        }
    }
}
