using AutoMapper;
using BusinessObject.Models;
using DTO.AccessoryDto;
using DTO.DiamondAccessoryDto;
using DTO.OrderDto;
using DTO.UserDto;

namespace Services.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserResponse>()
            .ReverseMap();
        CreateMap<OrderRequest, Order>()
            .ReverseMap();
        CreateMap<Order, OrderResponse>()
            .ReverseMap();
        CreateMap<AccessoryRequest, Accessory>()
            .ReverseMap();
        CreateMap<Accessory, AccessoryResponse>()
            .ReverseMap();
        CreateMap<DiamondAccessory, DiamondAccessoryResponse>()
            .ReverseMap();
    }
}