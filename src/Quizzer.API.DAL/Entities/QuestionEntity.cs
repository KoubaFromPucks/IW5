using AutoMapper;
using Quizzer.API.DAL.Enums;

namespace Quizzer.API.DAL.Entities; 
public record QuestionEntity : EntityBase{
    public required AnswerFormat Type { get; set; }
    public required string Text { get; set; }

    public Guid QuizId { get; set; }

    public QuizEntity? Quiz { get; set; }
    public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();
    public ICollection<UserAnswerEntity> UserAnswers { get; set; } = new List<UserAnswerEntity>();
}

public class QuestionMapperProfile : Profile {
    public QuestionMapperProfile() => CreateMap<QuestionEntity, QuestionEntity>();
}
