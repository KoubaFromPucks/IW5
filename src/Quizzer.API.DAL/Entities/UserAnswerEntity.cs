using AutoMapper;

namespace Quizzer.API.DAL.Entities; 
public record UserAnswerEntity : EntityBase {
    public required DateTime AnswerTime { get; set; }
    public double AnswerScore { get; set; }

    public Guid UserId { get; set; }
    public Guid QuestionId { get; set; }
    
    public UserEntity? User { get; set; }
    public QuestionEntity? Question { get; set; }
    public ICollection<SelectedAnswerEntity> SelectedAnswers { get; set; } = new List<SelectedAnswerEntity>();
}

public class UserAnswerMapperProfile : Profile {
    public UserAnswerMapperProfile() => CreateMap<UserAnswerEntity, UserAnswerEntity>();
}
