using AutoMapper;

namespace Quizzer.API.DAL.Entities; 
public record AnswerEntity : EntityBase {
    public required string Text { get; set; }
    public string? PictureUrl { get; set; }
    public required bool IsCorrect { get; set; }
    public int Order { get; set; }
    public Guid QuestionId { get; set; }
    public QuestionEntity? Question { get; set; }
    public ICollection<SelectedAnswerEntity> SelectedAnswers { get; set; } = new List<SelectedAnswerEntity>();
}

public class AnswerMapperProfile : Profile {
    public AnswerMapperProfile() => CreateMap<AnswerEntity, AnswerEntity>();
}
