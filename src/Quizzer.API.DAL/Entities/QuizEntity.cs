using AutoMapper;

namespace Quizzer.API.DAL.Entities; 
public record QuizEntity : EntityBase{
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public ICollection<CompletedQuizEntity> CompletedQuizzes { get; set; } = new List<CompletedQuizEntity>();
    public ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();
}

public class QuizMapperProfile : Profile {
    public QuizMapperProfile() => CreateMap<QuizEntity, QuizEntity>();
}
