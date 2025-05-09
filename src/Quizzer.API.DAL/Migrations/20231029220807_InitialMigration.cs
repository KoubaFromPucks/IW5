using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quizzer.API.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quiezzes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiezzes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Quiezzes_QuizEntityId",
                        column: x => x.QuizEntityId,
                        principalTable: "Quiezzes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_Quiezzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiezzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletedQuiezzes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score = table.Column<double>(type: "float", nullable: false),
                    InProgress = table.Column<bool>(type: "bit", nullable: false),
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuizEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedQuiezzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedQuiezzes_Quiezzes_QuizEntityId",
                        column: x => x.QuizEntityId,
                        principalTable: "Quiezzes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompletedQuiezzes_Quiezzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiezzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompletedQuiezzes_Users_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompletedQuiezzes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionEntityId",
                        column: x => x.QuestionEntityId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AnswerScore = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnswers_Questions_QuestionEntityId",
                        column: x => x.QuestionEntityId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswers_Users_UserEntityId",
                        column: x => x.UserEntityId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAnswers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelectedAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true),
                    UserAnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserAnswerEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectedAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectedAnswers_Answers_AnswerEntityId",
                        column: x => x.AnswerEntityId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SelectedAnswers_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SelectedAnswers_UserAnswers_UserAnswerEntityId",
                        column: x => x.UserAnswerEntityId,
                        principalTable: "UserAnswers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SelectedAnswers_UserAnswers_UserAnswerId",
                        column: x => x.UserAnswerId,
                        principalTable: "UserAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionEntityId",
                table: "Answers",
                column: "QuestionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedQuiezzes_QuizEntityId",
                table: "CompletedQuiezzes",
                column: "QuizEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedQuiezzes_QuizId",
                table: "CompletedQuiezzes",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedQuiezzes_UserEntityId",
                table: "CompletedQuiezzes",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedQuiezzes_UserId",
                table: "CompletedQuiezzes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizEntityId",
                table: "Questions",
                column: "QuizEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedAnswers_AnswerEntityId",
                table: "SelectedAnswers",
                column: "AnswerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedAnswers_AnswerId",
                table: "SelectedAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedAnswers_UserAnswerEntityId",
                table: "SelectedAnswers",
                column: "UserAnswerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SelectedAnswers_UserAnswerId",
                table: "SelectedAnswers",
                column: "UserAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionEntityId",
                table: "UserAnswers",
                column: "QuestionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionId",
                table: "UserAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserEntityId",
                table: "UserAnswers",
                column: "UserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserId",
                table: "UserAnswers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedQuiezzes");

            migrationBuilder.DropTable(
                name: "SelectedAnswers");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "UserAnswers");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Quiezzes");
        }
    }
}
