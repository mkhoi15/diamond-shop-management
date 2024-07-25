using BusinessObject.Enum;
using BusinessObject.Models;
using DTO.DeliveryDto;
using DTO.OrderDto;
using DTO.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Orders;


[Authorize(Roles = $"{nameof(Roles.Manager)},{nameof(Roles.User)}")]
public class OrderDeliveries : PageModel
{
    private readonly IDeliveryServices _deliveryServices;
    private readonly IOrderServices _orderServices;
    private readonly IUserServices _userServices;

    public OrderDeliveries(IDeliveryServices deliveryServices, IOrderServices orderServices, IUserServices userServices)
    {
        _deliveryServices = deliveryServices;
        _orderServices = orderServices;
        _userServices = userServices;
    }
    
    public OrderResponse OrderResponse { get; set; }
    public List<DeliveryResponse> Deliveries { get; set; }
    public UserResponse Customer { get; set; }

    public async Task OnGetAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var orderResponse = await _orderServices.GetOrderByIdAsync(orderId, cancellationToken);
        Customer = await _userServices.GetUserByIdAsync(orderResponse.CustomerId.ToString());
        OrderResponse = orderResponse;
        
        Deliveries = await _deliveryServices.GetDeliveriesByOrderId(orderId, cancellationToken);
    }
    
    public async Task<IActionResult> OnPostUpdateStatusAsync(Guid orderId, CancellationToken cancellationToken)
    {
        try
        {
            var orderUpdateDto = new Order()
            {
                Id = orderId,
                Status = "Delivered"
            };

            await _orderServices.UpdateOrderAsync(orderUpdateDto);

            // Refresh the order and deliveries data
            var orderResponse = await _orderServices.GetOrderByIdAsync(orderId, cancellationToken);
            Customer = await _userServices.GetUserByIdAsync(orderResponse.CustomerId.ToString());
            OrderResponse = orderResponse;

            Deliveries = await _deliveryServices.GetDeliveriesByOrderId(orderId, cancellationToken);

            return Page();
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", "An error occurred while create the order delivery.");
            return Page();
        }
        
    }
    
}