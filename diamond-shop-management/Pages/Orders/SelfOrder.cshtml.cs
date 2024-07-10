using System.Security.Claims;
using DTO.OrderDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Orders;

public class SelfOrder : PageModel
{
    private readonly IOrderServices _orderServices;

    public SelfOrder(IOrderServices orderServices)
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
    
    public async Task<IActionResult> OnGetAsync()
    {
        var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        if (customerId is null)
        {
            return RedirectToPage("/User/Login");
        }

        var allOrders = await _orderServices.GetOrdersByCustomerId(Guid.Parse(customerId!), default);

        if (!string.IsNullOrEmpty(SelectedStatus))
        {
            Orders = allOrders.Where(o => o.Status == SelectedStatus).ToList();
        }
        else
        {
            Orders = allOrders;
        }
        
        return Page();
    }
}