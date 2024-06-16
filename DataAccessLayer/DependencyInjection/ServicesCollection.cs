using DataAccessLayer.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.DependencyInjection;

public static class ServicesCollection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection collection)
    {
        collection.AddScoped<IUnitOfWork, UnitOfWork>();
        collection.AddScoped<IOrderDAO, OrderDAO>();
        collection.AddScoped<IOrderDetailDAO, OrderDetailDAO>();
        collection.AddScoped<IDiamondDAO, DiamondDAO>();
        collection.AddScoped<IPaperworkDAO, PaperworkDAO>();
        collection.AddScoped<IPromotionDAO, PromotionDAO>();
        return collection;
    }
}