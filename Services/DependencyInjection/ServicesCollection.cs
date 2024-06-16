using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstraction;
using Services.Mapper;

namespace Services.DependencyInjection;

public static class ServicesCollection
{
    public static IServiceCollection AddServices(this IServiceCollection collection)
    {
        collection.AddScoped<IUserServices, UserServices>()
            .AddScoped<IOrderServices, OrderServices>()
            .AddScoped<IOrderDetailServices, OrderDetailServices>()
            .AddScoped<IEmailServices, EmailServices>()
            .AddScoped<IPromotionServices, PromotionServices>()
            .AddScoped<IDiamondServices, DiamondServices>()
            .AddScoped<IPaperworkServices, PaperworkServices>()
            .AddConfigureAutoMapper();
        return collection;
    }

    private static IServiceCollection AddConfigureAutoMapper(this IServiceCollection collection)
    {
        collection.AddAutoMapper(typeof(MapperProfile));
        return collection;
    }
}