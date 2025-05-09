using System.ComponentModel.DataAnnotations;

namespace Quizzer.Common.Models; 
public record QuizDetailModel : IModel {
    public required Guid Id { get; init; }

    [Required]
    [QuizDateValidation(ErrorMessage = "Start time must be before End time")]
    public required DateTime StartTime { get; set; }

    [Required]
    [QuizDateValidation(ErrorMessage = "End time must be after Start time")]
    public required DateTime EndTime { get; set; }

    [Required]
    public required string? Name { get; set; }

    public string? Description { get; set; }

    public IList<QuestionDetailModel> Questions { get; init; } = new List<QuestionDetailModel>();

    public bool? IsPlayable { get; set; }

    public bool? CanSeeResults { get; set; }

    public bool IsEditable { get; set; }
}

class QuizDateValidation : ValidationAttribute {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
        var model = (QuizDetailModel)validationContext.ObjectInstance;

        if(DateTime.Compare(model.StartTime, model.EndTime) >= 0) {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
