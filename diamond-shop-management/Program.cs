using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer;
using DataAccessLayer.Abstraction;
using DataAccessLayer.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories.DependencyInjection;
using Services.Abstraction;
using Services.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(o =>
    {
        // options.ApplicationCookie?.Configure(o =>
        // {
        o.LoginPath = "/user/login";
        o.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        o.SlidingExpiration = true;
        //options.AccessDeniedPath = "/Forbidden/";
        // });
    });

builder.Services.AddIdentity<User, Role>(options =>
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

builder.Services.AddDbContext<DiamondShopDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddServices();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    await using var scope = app.Services.CreateAsyncScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    var user = await userManager.FindByNameAsync("admin");
    if (user is not null) return;
    var newUser = new User()
    {
        UserName = "admin",
        PhoneNumber = "0123456789",
        Email = "lqviet455@gmail.com",
        FullName = "Le Quoc Viet"
    };
    var result = await userManager.CreateAsync(newUser, "Admin@123");
    if (result.Succeeded)
    {
        await roleManager.CreateAsync(new Role()
        {
            Name = Roles.Admin.ToString()
        });
        await userManager.AddToRoleAsync(newUser, Roles.Admin.ToString());
    }
});

app.Run();