using System.Security.Claims;
using Azure;
using DTO.DeliveryDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Delivery;

public class DeliveryList : PageModel
{
    private readonly IDeliveryServices _deliveryServices;
    private readonly IUserServices _userServices;

    public DeliveryList(IDeliveryServices deliveryServices, IUserServices userServices)
    {
        _deliveryServices = deliveryServices;
        _userServices = userServices;
    }
    
    public List<DeliveryResponse> DeliveryResponses { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var deliveryStaffId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userServices.GetUserByIdAsync(deliveryStaffId);

            if (user is null)
            {
                return NotFound("Delivery Staff not exist");
            }
            
            DeliveryResponses = await _deliveryServices.GetDeliveriesByDeliveryManId(Guid.Parse(deliveryStaffId), cancellationToken);

            return Page();

        }
        catch (Exception e)
        {
            return NotFound("An error occurred while retrieving the delivery list.");
        }
        

    }
}