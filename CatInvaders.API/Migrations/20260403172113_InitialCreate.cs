using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatInvaders.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HighScores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    LevelReached = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HighScores", x => x.Id);
                    table.CheckConstraint("CK_HighScores_Score", "\"Score\" >= 0");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HighScores_Score_CreatedAtUtc",
                table: "HighScores",
                columns: new[] { "Score", "CreatedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HighScores");
        }
    }
}
