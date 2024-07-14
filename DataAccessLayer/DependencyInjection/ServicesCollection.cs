using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.DependencyInjection;

public static class ServicesCollection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddScoped<IUnitOfWork, UnitOfWork>();
        collection.AddScoped<IOrderDAO, OrderDAO>();
        collection.AddScoped<IOrderDetailDAO, OrderDetailDAO>();
        collection.AddScoped<IDiamondDAO, DiamondDAO>();
        collection.AddScoped<IPaperworkDAO, PaperworkDAO>();
        collection.AddScoped<IPromotionDAO, PromotionDAO>();
        collection.AddScoped<IAccessoryDAO, AccessoryDAO>();
        collection.AddScoped<IDiamondAccessoryDAO, DiamondAccessoryDAO>();
        collection.AddScoped<IMediaDAO, MediaDAO>();
        collection.AddScoped<IDeliveryDAO, DeliveryDAO>();
        AddIdentity(collection);
        collection.AddDbContext<DiamondShopDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        return collection;
    }

    private static void AddIdentity(IServiceCollection collection)
    {
        collection.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = true;
            })
            .AddEntityFrameworkStores<DiamondShopDbContext>()
            .AddUserStore<UserStore<User, Role
                , DiamondShopDbContext, Guid>>()
            .AddRoleStore<RoleStore<Role, DiamondShopDbContext, Guid>>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
    }
}