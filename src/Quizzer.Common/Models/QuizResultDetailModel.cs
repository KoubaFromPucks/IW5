namespace Quizzer.Common.Models; 
public record QuizResultDetailModel : IModel {
    public required Guid Id { get; init; }

    public double UserScore { get; set; }

    public double MaxScore { get; set; }

    public required string? Name { get; init; }

    public string? Description { get; init; }
}
