using AutoMapper;

namespace Quizzer.API.DAL.Entities; 
public record CompletedQuizEntity : EntityBase{
    public required double Score { get; set; }
    public bool InProgress { get; set; }

    public Guid QuizId { get; set; }
    public Guid UserId { get; set; }

    public QuizEntity? Quiz { get; set; }
    public UserEntity? User { get; set; }
}

public class CompletedQuizMapperProfile : Profile {
    public CompletedQuizMapperProfile() => CreateMap<CompletedQuizEntity, CompletedQuizEntity>();
}
