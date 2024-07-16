using AutoMapper;
using BusinessObject.Models;
using DTO.DiamondAccessoryDto;
using DTO.OrderDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Orders;

public class OrderDetails : PageModel
{
    private readonly IOrderServices _orderServices;
    private readonly IUserServices _userServices;
    private readonly IMapper _mapper;
    private readonly IDiamondAccessoryServices _diamondAccessoryServices;
    private readonly IDiamondServices _diamondServices;

    public OrderDetails(IOrderServices orderServices, IUserServices userServices, IMapper mapper, IDiamondAccessoryServices diamondAccessoryServices, IDiamondServices diamondServices)
    {
        _orderServices = orderServices;
        _userServices = userServices;
        _mapper = mapper;
        _diamondAccessoryServices = diamondAccessoryServices;
        _diamondServices = diamondServices;
    }
    
    public OrderResponse Order { get; set; }
    

    public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var orderResponse = await _orderServices.GetOrderByIdAsync(id, cancellationToken);
            if (orderResponse == null)
            {
                return NotFound("Order not found.");
            }

            var customer = await _userServices.GetUserByIdAsync(orderResponse.CustomerId.ToString());
            orderResponse.CustomerName = customer.FullName;
            orderResponse.PhoneNumber = customer.PhoneNumber;
            
            Order = orderResponse;

        }
        catch (Exception e)
        {
            return NotFound("An error occurred while retrieving the order details.");
        }

        return Page();
    }
    
    
}