using AutoMapper;
using Quizzer.API.DAL.Entities;
using Quizzer.Common.Models;
using Quizzer.Extensions;

namespace Quizzer.API.BL.MapperProfiles; 
public class AnswerMapperProfile : Profile {
    public AnswerMapperProfile() {
        CreateMap<AnswerEntity, AnswerDetailModel>()
            .Ignore(dst => dst.Type)
            .Ignore(dst => dst.SelectedOrder)
            .Ignore(dst => dst.IsUserSelected)
            .Ignore(dst => dst.IsQuizEditable);

        CreateMap<AnswerEntity, AnswerResultModel>()
            .MapMember(dst => dst.CorrectOrder, src => src.Order)
            .Ignore(dst => dst.IsAnswered)
            .Ignore(dst => dst.Type)
            .Ignore(dst => dst.SelectedOrder);


        CreateMap<AnswerDetailModel, AnswerEntity>()
            .Ignore(dst => dst.Question)
            .Ignore(dst => dst.SelectedAnswers);
    }
}
