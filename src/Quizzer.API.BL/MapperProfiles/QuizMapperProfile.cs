using AutoMapper;
using Quizzer.API.DAL.Entities;
using Quizzer.Common.Models;
using Quizzer.Extensions;

namespace Quizzer.API.BL.MapperProfiles; 
public class QuizMapperProfile : Profile {
    public QuizMapperProfile() {
        CreateMap<QuizEntity, QuizListModel>()
            .Ignore(dst => dst.UserScore)
            .Ignore(dst => dst.IsCompleted)
            .Ignore(dst => dst.MaxScore);

        CreateMap<QuizEntity, QuizDetailModel>()
            .MapMember(dst => dst.Questions, src => src.Questions)
            .Ignore(dst => dst.IsPlayable)
            .Ignore(dst => dst.IsEditable)
            .Ignore(dst => dst.CanSeeResults);

        CreateMap<QuizEntity, QuizResultDetailModel>()
            .Ignore(dst => dst.UserScore)
            .Ignore(dst => dst.MaxScore);

        CreateMap<QuizDetailModel, QuizEntity>()
            .Ignore(dst => dst.Questions)
            .Ignore(dst => dst.CompletedQuizzes);

        CreateMap<CompletedQuizEntity, QuizListModel>()
            .Ignore(dst => dst.StartTime)
            .Ignore(dst => dst.EndTime)
            .Ignore(dst => dst.Name)
            .Ignore(dst => dst.Description)
            .Ignore(dst => dst.IsCompleted)
            .Ignore(dst => dst.UserScore)
            .Ignore(dst => dst.MaxScore);
    }
}
