using System.Security.Claims;
using Azure;
using BusinessObject.Enum;
using BusinessObject.Models;
using DTO.DeliveryDto;
using DTO.OrderDto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Delivery;

public class DeliveryList : PageModel
{
    private readonly IDeliveryServices _deliveryServices;
    private readonly IUserServices _userServices;
    private readonly IOrderServices _orderServices;

    public DeliveryList(IDeliveryServices deliveryServices, IUserServices userServices, IOrderServices orderServices)
    {
        _deliveryServices = deliveryServices;
        _userServices = userServices;
        _orderServices = orderServices;
    }
    

    public List<OrderResponse> Orders { get; set; }
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var deliveryStaffId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userServices.GetUserByIdAsync(deliveryStaffId);

            if (user.Role != Roles.Delivery.ToString())
            {
                return NotFound("You are not a delivery staff.");
            }

            if (user is null)
            {
                return NotFound("Delivery Staff not exist");
            }
            
            var orders = await _orderServices.GetAllOrdersAsync();
            Orders = orders.Where(order => order.Deliveries.Any(delivery => delivery.DeliveryManId == Guid.Parse(deliveryStaffId))).ToList();

            return Page();

        }
        catch (Exception e)
        {
            return NotFound("An error occurred while retrieving the delivery list.");
        }
        

    }
}