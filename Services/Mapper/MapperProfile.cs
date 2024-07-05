using AutoMapper;
using BusinessObject.Models;
using DTO.DiamondDto;
using DTO.AccessoryDto;
using DTO.DiamondAccessoryDto;
using DTO.OrderDto;
using DTO.PaperworkDto;
using DTO.UserDto;
using DTO.Media;
using DTO.PromotionDto;

namespace Services.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserResponse>()
            .ReverseMap();
        CreateMap<OrderRequest, Order>()
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
            .ReverseMap();
        CreateMap<Order, OrderResponse>()
            .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
            .ReverseMap();
        
        // Mapping for OrderDetailRequest to OrderDetail
        CreateMap<OrderDetailRequest, OrderDetail>()
            .ReverseMap();
        
        // Add this mapping
        CreateMap<OrderDetail, OrderDetailResponse>()
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
            .ForMember(dest => dest.PromotionName, opt => opt.MapFrom(src => src.Promotion.Name))
            .ForMember(dest => dest.MediaUrl, opt => opt.MapFrom(src => src.Media.Url))
            .ReverseMap();
        CreateMap<DiamondAccessory, DiamondAccessoryResponse>()
            .ReverseMap();
        CreateMap<MediaRequest, Media>()
            .ReverseMap();
        CreateMap<Media, MediaResponse>()
            .ReverseMap();
        CreateMap<DiamondAccessoryRequest, DiamondAccessory>()
            .ReverseMap();
        CreateMap<Promotion, PromotionResponse>()
            .ReverseMap();
    }
}