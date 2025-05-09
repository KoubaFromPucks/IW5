namespace Quizzer.Common.Models; 
public record QuizListModel : IModel {
    public required Guid Id { get; init; }

    public required DateTime StartTime { get; set; }

    public required DateTime EndTime { get; set; }

    public required string Name { get; set; }
    
    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public int UserScore { get; set; }

    public int MaxScore { get; set; }
}
