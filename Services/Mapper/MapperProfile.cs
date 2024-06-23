using AutoMapper;
using BusinessObject.Models;
using DTO.DiamondDto;
using DTO.AccessoryDto;
using DTO.DiamondAccessoryDto;
using DTO.OrderDto;
using DTO.PaperworkDto;
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
        CreateMap<DiamondRequest,Diamond>()
            .ReverseMap();
        CreateMap<Diamond, DiamondResponse>()
            .ReverseMap();
        CreateMap<PaperworkRequest, PaperWork>()
            .ReverseMap();
        CreateMap<PaperWork,PaperworkResponse>()
            .ReverseMap();
        CreateMap<AccessoryRequest, Accessory>()
            .ReverseMap();
        CreateMap<Accessory, AccessoryResponse>()
            .ReverseMap();
        CreateMap<DiamondAccessory, DiamondAccessoryResponse>()
            .ReverseMap();
    }
}