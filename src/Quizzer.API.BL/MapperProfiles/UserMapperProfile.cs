using AutoMapper;
using Quizzer.API.DAL.Entities;
using Quizzer.Common.Models;
using Quizzer.Extensions;

namespace Quizzer.API.BL.MapperProfiles; 
public class UserMapperProfile : Profile {
    public UserMapperProfile() {
        //CreateMap<UserEntity, UserDetailModel>()
        //    .MapMember(dst => dst.Quizes, src => src.CompletedQuizzes);
        CreateMap<UserEntity, UserDetailModel>()
            .Ignore(dst => dst.Quizes);

        CreateMap<UserEntity, UserListModel>();

        CreateMap<UserDetailModel, UserEntity>()
            .Ignore(dst => dst.CompletedQuizzes)
            .Ignore(dst => dst.UserAnswers);
    }
}
