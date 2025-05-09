using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizzer.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fixDoubledForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answers_Questions_QuestionEntityId",
                table: "Answers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompletedQuiezzes_Quiezzes_QuizEntityId",
                table: "CompletedQuiezzes");

            migrationBuilder.DropForeignKey(
                name: "FK_CompletedQuiezzes_Users_UserEntityId",
                table: "CompletedQuiezzes");

            migrationBuilder.DropForeignKey(
                name: "FK_SelectedAnswers_Answers_AnswerEntityId",
                table: "SelectedAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_SelectedAnswers_UserAnswers_UserAnswerEntityId",
                table: "SelectedAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Questions_QuestionEntityId",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Users_UserEntityId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_QuestionEntityId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_UserEntityId",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_SelectedAnswers_AnswerEntityId",
                table: "SelectedAnswers");

            migrationBuilder.DropIndex(
                name: "IX_SelectedAnswers_UserAnswerEntityId",
                table: "SelectedAnswers");

            migrationBuilder.DropIndex(
                name: "IX_CompletedQuiezzes_QuizEntityId",
                table: "CompletedQuiezzes");

            migrationBuilder.DropIndex(
                name: "IX_CompletedQuiezzes_UserEntityId",
                table: "CompletedQuiezzes");

            migrationBuilder.DropIndex(
                name: "IX_Answers_QuestionEntityId",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "QuestionEntityId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "AnswerEntityId",
                table: "SelectedAnswers");

            migrationBuilder.DropColumn(
                name: "UserAnswerEntityId",
                table: "SelectedAnswers");

            migrationBuilder.DropColumn(
                name: "QuizEntityId",
                table: "CompletedQuiezzes");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "CompletedQuiezzes");

            migrationBuilder.DropColumn(
                name: "QuestionEntityId",
                table: "Answers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "QuestionEntityId",
                table: "UserAnswers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "UserAnswers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AnswerEntityId",
                table: "SelectedAnswers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserAnswerEntityId",
                table: "SelectedAnswers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QuizEntityId",
                table: "CompletedQuiezzes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "CompletedQuiezzes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "QuestionEntityId",
                table: "Answers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionEntityId",
                table: "UserAnswers",
                column: "QuestionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserEntityId",
                table: "UserAnswers",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedAnswers_AnswerEntityId",
                table: "SelectedAnswers",
                column: "AnswerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedAnswers_UserAnswerEntityId",
                table: "SelectedAnswers",
                column: "UserAnswerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedQuiezzes_QuizEntityId",
                table: "CompletedQuiezzes",
                column: "QuizEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedQuiezzes_UserEntityId",
                table: "CompletedQuiezzes",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionEntityId",
                table: "Answers",
                column: "QuestionEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Answers_Questions_QuestionEntityId",
                table: "Answers",
                column: "QuestionEntityId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedQuiezzes_Quiezzes_QuizEntityId",
                table: "CompletedQuiezzes",
                column: "QuizEntityId",
                principalTable: "Quiezzes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedQuiezzes_Users_UserEntityId",
                table: "CompletedQuiezzes",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedAnswers_Answers_AnswerEntityId",
                table: "SelectedAnswers",
                column: "AnswerEntityId",
                principalTable: "Answers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SelectedAnswers_UserAnswers_UserAnswerEntityId",
                table: "SelectedAnswers",
                column: "UserAnswerEntityId",
                principalTable: "UserAnswers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Questions_QuestionEntityId",
                table: "UserAnswers",
                column: "QuestionEntityId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Users_UserEntityId",
                table: "UserAnswers",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
