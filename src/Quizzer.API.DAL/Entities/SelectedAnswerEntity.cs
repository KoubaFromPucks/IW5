using AutoMapper;

namespace Quizzer.API.DAL.Entities; 
public record SelectedAnswerEntity : EntityBase {
    public int? Order { get; set; }

    public Guid UserAnswerId { get; set; }
    public Guid AnswerId { get; set; }

    public AnswerEntity? Answer { get; set; }
    public UserAnswerEntity? UserAnswer { get; set; }
}

public class SelectedAnswerMapperProfile : Profile {
    public SelectedAnswerMapperProfile() => CreateMap<SelectedAnswerEntity, SelectedAnswerEntity>();
}
