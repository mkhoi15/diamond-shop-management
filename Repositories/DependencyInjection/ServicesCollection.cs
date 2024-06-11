using Microsoft.Extensions.DependencyInjection;
using Repositories.Abstraction;

namespace Repositories.DependencyInjection;

public static class ServicesCollection
{
    public static IServiceCollection AddRepositories(this IServiceCollection collection)
    {
        collection.AddScoped<IOrderRepository, OrderRepository>();
        collection.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        collection.AddScoped<IAccessoryRepository, AccessoryRepository>();
        collection.AddScoped<IDiamondAccessoryRepository, DiamondAccessoryRepository>();
        return collection;
    }
}