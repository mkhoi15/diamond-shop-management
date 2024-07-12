using System.ComponentModel.DataAnnotations;
using DTO.Revenue;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Admin;

public class Revenue : PageModel
{
    private readonly IRevenueServices _revenueServices;
    public List<RevenueResponse> RevenueResponses { get; set; } = new List<RevenueResponse>();
    public decimal TotalRevenue { get; set; }
    public int Year { get; set; } = DateTime.Now.Year;
    
    public Revenue(IRevenueServices revenueServices)
    {
        _revenueServices = revenueServices;
    }

    
    
    public async Task OnGet(int? year = 2024)
    {
        RevenueResponses = await _revenueServices.GetRevenueByYear(year);
        TotalRevenue = RevenueResponses.Sum(revenue => revenue.TotalRevenue);
    }
}