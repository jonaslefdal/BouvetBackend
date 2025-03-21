﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BouvetBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddMoneySaved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "MoneySaved",
                table: "transportEntries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoneySaved",
                table: "transportEntries");
        }
    }
}
