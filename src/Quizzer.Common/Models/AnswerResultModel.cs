using Quizzer.API.DAL.Enums;

namespace Quizzer.Common.Models;
public record AnswerResultModel : IModel {
    public required Guid Id { get; init; }

    public required bool IsCorrect { get; set; }

    public int CorrectOrder { get; set; }

    public bool IsAnswered { get; set; }

    public AnswerFormat Type { get; set; }

    public required string Text { get; set; }

    public string? PictureUrl { get; set; }

    public int? SelectedOrder { get; set; }
}