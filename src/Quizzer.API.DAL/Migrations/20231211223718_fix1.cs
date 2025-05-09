using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizzer.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Quiezzes_QuizEntityId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_QuizEntityId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "QuizEntityId",
                table: "Questions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuizEntityId",
                table: "Questions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizEntityId",
                table: "Questions",
                column: "QuizEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Quiezzes_QuizEntityId",
                table: "Questions",
                column: "QuizEntityId",
                principalTable: "Quiezzes",
                principalColumn: "Id");
        }
    }
}
