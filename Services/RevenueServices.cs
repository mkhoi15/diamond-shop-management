using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Common;
using DTO.Enum;
using DTO.Revenue;
using DTO.UserDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstraction;
using Services.Abstraction;

namespace Services;

public class RevenueServices : IRevenueServices
{
    private readonly IOrderRepository _orderServices;
    private readonly UserManager<User> _userManager;

    public RevenueServices(IOrderRepository orderServices, UserManager<User> userManager)
    {
        _orderServices = orderServices;
        _userManager = userManager;
    }

    public async Task<List<RevenueResponse>> GetRevenueByYear(int? year = null)
    {
        var query = _orderServices.FindAll()
            .Where(o => o.Status == OrderStatus.Delivered.ToString())
            .WhereIf(year != null, order => order.Date.Year == year);
        
        var list = await query.GroupBy(order => order.Date.Month)
            .Select(group => new RevenueResponse
            {
                TotalRevenue = group.Sum(order => order.TotalPrice),
                TotalOrder = group.Count(),
                Month = group.Key.ToString(),
                Orders = group.Select(order => new OrderExcel
                {
                    Id = order.Id,
                    UserName = order.Customer.UserName,
                    Email = order.Customer.Email,
                    TotalPrice = order.TotalPrice,
                    Date = order.Date
                }).ToList()
            }).ToListAsync();
        
        // Calculate revenue growth rate
        for (int i = 1; i < list.Count; i++)
        {
            list[i].RevenueGrowth = (list[i].TotalRevenue - list[i - 1].TotalRevenue) / list[i - 1].TotalRevenue * 100;
        }
        return list;
    }

    public async Task<List<UserStatistic>> GetUserStatisticsByYear(int? year = null)
    {
        var query = _userManager.Users
            .AsNoTracking();
        if (year != null)
        {
            query = query.Where(user => user.CreatedAt.Year == year.Value);
        }
        
        var listUser = new List<User>();

        foreach (var user in await query.ToListAsync())
        {
            if (await _userManager.IsInRoleAsync(user, Roles.User.ToString()))
            {
                listUser.Add(user);
            }
        }
        
        var list = listUser
            .GroupBy(user => user.CreatedAt.Month)
            .Select(group => new UserStatistic
            {
                Month = group.Key.ToString(),
                NewUsers = group.Count(),
                TotalUsers = group.Select(user => user.Id).Distinct().Count() // Total users in the system
            }).ToList();
        
        return list;
    }
    
}