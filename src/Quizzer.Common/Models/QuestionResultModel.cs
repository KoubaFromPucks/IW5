using Quizzer.API.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Quizzer.Common.Models;
public record QuestionResultModel : IModel {
    public required Guid Id { get; init; }

    [Required]
    public required string Text { get; set; }

    public required AnswerFormat Type { get; set; }

    public bool IsAnswered { get; set; }

    public bool IsCorrect { get; set; }

    public IList<AnswerResultModel> Answers { get; set; } = new List<AnswerResultModel>();
}