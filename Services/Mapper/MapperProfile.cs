using AutoMapper;
using BusinessObject.Models;
using DTO.UserDto;

namespace Services.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserResponse>()
            .ReverseMap();
    }
}