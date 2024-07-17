using BusinessObject.Enum;
using DTO.OrderDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Delivery;

[Authorize(Roles = nameof(Roles.Delivery))]
public class DeliveryOrderDetails : PageModel
{
    private readonly IOrderServices _orderServices;

    public DeliveryOrderDetails(IOrderServices orderServices)
    {
        _orderServices = orderServices;
    }
    
    public OrderResponse Order { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            Order = await _orderServices.GetOrderByIdAsync(id, cancellationToken);
            
            if (Order == null)
            {
                return NotFound("Not found order");
            }

            return Page();

        }
        catch (Exception e)
        {
            return NotFound("An error occurred while retrieving the order details.");
        }
    }
}