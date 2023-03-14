using ASMC6.Shared.Dtos;
using ASMC6.Shared.Entities;

using AutoMapper;

namespace ASMC6.Server.Infrastructures.Extensions.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, UserEntity>().ReverseMap();
    }
}