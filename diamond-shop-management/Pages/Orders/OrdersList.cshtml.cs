using DTO.OrderDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Orders;

public class OrdersList : PageModel
{
    private readonly IOrderServices _orderServices;

    public OrdersList(IOrderServices orderServices)
    {
        _orderServices = orderServices;
        StatusList = new List<SelectListItem>
        {
            new SelectListItem { Value = "Pending", Text = "Pending" },
            new SelectListItem { Value = "Shipped", Text = "Shipped" },
            new SelectListItem { Value = "Delivered", Text = "Delivered" },
            new SelectListItem { Value = "Cancelled", Text = "Cancelled" }
        };
    }
    
    public List<OrderResponse> Orders { get; set; }

    [BindProperty(SupportsGet = true)]
    public string SelectedStatus { get; set; }

    public List<SelectListItem> StatusList { get; set; } 
    
    [BindProperty(SupportsGet = true)]
    public string SearchString { get; set; }
    

    public async Task OnGetAsync()
    {
        var allOrders = await _orderServices.GetAllOrdersAsync();

        if (!string.IsNullOrEmpty(SelectedStatus) && !string.IsNullOrEmpty(SearchString))
        {
            Orders = allOrders.Where(o =>
                o.Status == SelectedStatus &&
                ((o.Customer?.Email?.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
                 (o.Customer?.FullName?.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ?? false))
            ).ToList();
        }
        else if (!string.IsNullOrEmpty(SelectedStatus))
        {
            Orders = allOrders.Where(o => o.Status == SelectedStatus).ToList();
        }
        else if (!string.IsNullOrEmpty(SearchString))
        {
            Orders = allOrders.Where(o =>
                (o.Customer?.Email?.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (o.Customer?.FullName?.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ?? false)
            ).ToList();
        }
        else
        {
            Orders = allOrders;
        }
        
    }
}