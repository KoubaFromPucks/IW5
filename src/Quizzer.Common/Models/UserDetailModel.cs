using System.ComponentModel.DataAnnotations;

namespace Quizzer.Common.Models; 
public record UserDetailModel : IModel {
    public required Guid Id { get; init; }

    [Required]
    [MinLength(2)]
    public required string? Name { get; set; }

    public string? ProfilePictureUrl { get; set; }

    public IList<QuizListModel> Quizes { get; set; } = new List<QuizListModel>();
}
