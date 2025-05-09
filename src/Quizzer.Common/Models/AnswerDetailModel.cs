using Microsoft.IdentityModel.Tokens;
using Quizzer.API.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Quizzer.Common.Models; 
public record AnswerDetailModel : IModel {
    public required Guid Id { get; init; }

    public required Guid QuestionId { get; init; }

    public AnswerFormat Type { get; set; }

    [AnswerNotEmptyValidation(ErrorMessage = "Either text or picture must be filled")]
    public required string Text { get; set; }

    public required bool IsCorrect { get; set; }

    [AnswerNotEmptyValidation(ErrorMessage = "Either text or picture must be filled")]
    public string? PictureUrl { get; set; }

    public int Order {  get; set; }
    
    public int? SelectedOrder { get; set; }

    public bool IsUserSelected { get; set; }

    public bool IsQuizEditable { get; set; }
}

class AnswerNotEmptyValidation : ValidationAttribute {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
        var model = (AnswerDetailModel)validationContext.ObjectInstance;

        if (model.PictureUrl.IsNullOrEmpty() && model.Text.IsNullOrEmpty()) {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}