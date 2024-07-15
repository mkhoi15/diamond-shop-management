using DataAccessLayer.Common;
using DTO.Enum;
using DTO.Revenue;
using Microsoft.EntityFrameworkCore;
using Repositories.Abstraction;
using Services.Abstraction;

namespace Services;

public class RevenueServices : IRevenueServices
{
    private readonly IOrderRepository _orderServices;

    public RevenueServices(IOrderRepository orderServices)
    {
        _orderServices = orderServices;
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
                Month = group.Key.ToString()
            }).ToListAsync();

        return list;
    }
}