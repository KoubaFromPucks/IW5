using AutoMapper;
using Quizzer.API.DAL.Entities;
using Quizzer.Common.Models;
using Quizzer.Extensions;

namespace Quizzer.API.BL.MapperProfiles;
public class QuestionMapperProfile : Profile {
    public QuestionMapperProfile() {
        CreateMap<QuestionEntity, QuestionDetailModel>()
            .MapMember(dst => dst.Answers, src => src.Answers)
            .Ignore(dst => dst.IsAnswered)
            .MapMember(dst => dst.QuestionType, src => src.Type)
            .Ignore(dst => dst.IsQuizEditable);

        CreateMap<QuestionEntity, QuestionResultModel>()
            .MapMember(dst => dst.Answers, src => src.Answers)
            .Ignore(dst => dst.IsAnswered)
            .Ignore(dst => dst.IsCorrect);

        CreateMap<QuestionDetailModel, QuestionEntity>()
            .Ignore(dst => dst.Answers)
            .Ignore(dst => dst.Quiz)
            .Ignore(dst => dst.UserAnswers)
            .MapMember(dst => dst.Type, src => src.QuestionType);
    }
}
