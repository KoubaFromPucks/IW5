namespace Quizzer.Common.Models; 
public record UserListModel : IModel {
    public required Guid Id { get; init; }

    public required string Name { get; set; }
    
    public string? ProfilePictureUrl { get; set; }
}
