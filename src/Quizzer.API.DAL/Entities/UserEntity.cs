using AutoMapper;

namespace Quizzer.API.DAL.Entities; 
public record UserEntity : EntityBase{
    public required string Name { get; set; }
    public string? ProfilePictureUrl {  get; set; }

    public ICollection<CompletedQuizEntity> CompletedQuizzes { get; set; } = new List<CompletedQuizEntity>();
    public ICollection<UserAnswerEntity> UserAnswers { get; set; } = new HashSet<UserAnswerEntity>();
}

public class UserMapperProfile : Profile {
    public UserMapperProfile() => CreateMap<UserEntity, UserEntity>();
}
