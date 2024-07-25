using BusinessObject.Enum;
using BusinessObject.Models;
using DTO.DeliveryDto;
using DTO.DiamondDto;
using DTO.OrderDto;
using DTO.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Delivery;

[Authorize(Roles = nameof(Roles.Manager))]
public class CreateDelivery : PageModel
{
    private readonly IOrderServices _orderServices;
    private readonly IUserServices _userServices;
    private readonly IDeliveryServices _deliveryServices;

    public CreateDelivery(IOrderServices orderServices, IUserServices userServices, IDeliveryServices deliveryServices)
    {
        _orderServices = orderServices;
        _userServices = userServices;
        _deliveryServices = deliveryServices;
    }
    public List<DiamondResponse> DiamondResponses { get; set; }
    public OrderResponse OrderResponse { get; set; }
    public List<UserResponse> DeliveryMen { get; set; }
    
    [BindProperty]
    public DeliveryRequest DeliveryRequest { get; set; }
    
    public async Task OnGet(Guid id, CancellationToken cancellationToken)
    {
        DiamondResponses = await _orderServices.GetDiamondsByOrderId(id, cancellationToken);
        var orderResponse = await _orderServices.GetOrderByIdAsync(id, cancellationToken);
        var customer = await _userServices.GetUserByIdAsync(orderResponse.CustomerId.ToString());
        
        orderResponse.CustomerName = customer.FullName;
        orderResponse.PhoneNumber = customer.PhoneNumber;
        
        OrderResponse = orderResponse;
        
        DeliveryMen = await _userServices.GetDeliveryMenAsync(cancellationToken);
    }
    
    public async Task<IActionResult> OnPostAsync(Guid id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            DeliveryMen = await _userServices.GetDeliveryMenAsync(cancellationToken);
            return Page();
        }

        DeliveryRequest.OrderId = id;
        DeliveryRequest.CreatedAt = DateTime.Now;
        var deliveryResponse = await _deliveryServices.CreateDeliveryAsync(DeliveryRequest);

        var updateOrder = new Order()
        {
            Id = id,
            Status = "Shipped"
        };

        await _orderServices.UpdateOrderAsync(updateOrder);
            
        return RedirectToPage("/Orders/OrderDeliveries", new { orderId = deliveryResponse.OrderId });
    }
    
}