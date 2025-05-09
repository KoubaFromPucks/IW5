using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizzer.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class SelectedAnswersCascade2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedAnswers_UserAnswers_UserAnswerId",
                table: "SelectedAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedAnswers_UserAnswers_UserAnswerId",
                table: "SelectedAnswers",
                column: "UserAnswerId",
                principalTable: "UserAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SelectedAnswers_UserAnswers_UserAnswerId",
                table: "SelectedAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedAnswers_UserAnswers_UserAnswerId",
                table: "SelectedAnswers",
                column: "UserAnswerId",
                principalTable: "UserAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
