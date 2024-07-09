using BusinessObject.Models;
using DTO.OrderDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Orders;

public class OrderDetails : PageModel
{
    private readonly IOrderServices _orderServices;
    private readonly IUserServices _userServices;

    public OrderDetails(IOrderServices orderServices, IUserServices userServices)
    {
        _orderServices = orderServices;
        _userServices = userServices;
    }
    
    public OrderResponse Order { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var orderResponse = await _orderServices.GetOrderByIdAsync(id, cancellationToken);
            if (orderResponse == null)
            {
                return NotFound();
            }

            var customer = await _userServices.GetUserByIdAsync(orderResponse.CustomerId.ToString());
            orderResponse.CustomerName = customer.FullName;
            orderResponse.PhoneNumber = customer.PhoneNumber;
            
            Order = orderResponse;

        }
        catch (Exception e)
        {
            return NotFound();
        }

        return Page();
    }
    
    public async Task<IActionResult> OnPostCancelOrderAsync(Guid orderId, CancellationToken cancellationToken)
    {
        try
        {
            
            
            
            var order = new Order()
            {
                Id = orderId,
                Status = "Cancelled"
            };
            var success = await _orderServices.UpdateOrderAsync(order);
            if (success == null)
            {
                ModelState.AddModelError("", "Failed to cancel the order.");
                return Page();
            }

            return RedirectToPage("/Orders/OrdersList");
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "An error occurred while cancelling the order.");
            return Page();
        }
    }
    
    
    
}