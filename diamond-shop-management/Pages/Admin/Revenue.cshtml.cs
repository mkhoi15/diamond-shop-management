using DTO.Revenue;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Admin;

public class Revenue : PageModel
{
    private readonly IRevenueServices _revenueServices;

    public Revenue(IRevenueServices revenueServices)
    {
        _revenueServices = revenueServices;
    }

    public List<RevenueResponse> RevenueResponses { get; set; } = new List<RevenueResponse>();
    
    
    public async Task OnGet()
    {
        var year = DateTime.Today.Year;

        RevenueResponses = await _revenueServices.GetRevenueByYear(year);
    }
}