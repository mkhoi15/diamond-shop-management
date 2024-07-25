using BusinessObject.Enum;
using BusinessObject.Models;
using DTO.DeliveryDto;
using DTO.OrderDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Delivery;

[Authorize(Roles = nameof(Roles.Delivery))]
public class UpdateStatus : PageModel
{
    private readonly IDeliveryServices _deliveryServices;
    private readonly IOrderServices _orderServices;

    public UpdateStatus(IDeliveryServices deliveryServices, IOrderServices orderServices)
    {
        _deliveryServices = deliveryServices;
        _orderServices = orderServices;
    }
    
    [BindProperty]
    public DeliveryResponse? Delivery { get; set; }
    
    [BindProperty]
    public string Status { get; set; } 
    
    public OrderResponse? Order { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
    {
        Delivery = await _deliveryServices.GetDeliveryByIdAsync(id, cancellationToken);

        if (Delivery == null)
        {
            return NotFound("Delivery not found");
        }
        
        var order = await _orderServices.GetOrderByIdAsync(Delivery.OrderId, cancellationToken);
        Order = order;
        if (order.Status == "Cancelled" || order.Status == "Delivered")
        {
            ModelState.AddModelError("", "You cannot update the delivery status.");
            return Page();
        }

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(Guid id, CancellationToken cancellationToken)
    {
        
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Invalid input. Please check again.");
            return Page();
        }

        var delivery = await _deliveryServices.GetDeliveryByIdAsync(id, cancellationToken);

        if (delivery == null)
        {
            return NotFound("Delivery not found.");
        }

        delivery.Status = Status;

        var result = await _deliveryServices.UpdateDeliveryAsync(delivery);

        if (result != null)
        {
            return RedirectToPage("/Delivery/DeliveryOrderDetails", new { id = delivery.OrderId });
        }

        ModelState.AddModelError("", "An error occurred while updating the delivery status.");
        return Page();
    }
    
}