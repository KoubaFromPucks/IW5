using Quizzer.API.DAL.Enums;
using System.ComponentModel.DataAnnotations;

namespace Quizzer.Common.Models; 
public record QuestionDetailModel : IModel {
    public required Guid Id { get; init; }

    [Required]
    public required string Text { get; set; }

    public bool IsAnswered { get; set; }

    [Required]
    public AnswerFormat QuestionType { get; set; }

    [Required]
    public Guid QuizId { get; set; }

    public bool IsQuizEditable { get; set; }

    public IList<AnswerDetailModel> Answers { get; set; } = new List<AnswerDetailModel>();

}
