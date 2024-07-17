using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.Models;
using DTO.DiamondAccessoryDto;
using DTO.DiamondDto;
using DTO.OrderDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Orders;


[Authorize(Roles = $"{nameof(Roles.Manager)},{nameof(Roles.User)}")]
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
    
    public List<DiamondResponse> DiamondResponses { get; set; }
    

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

            DiamondResponses = await _orderServices.GetDiamondsByOrderId(id, cancellationToken);
            
            Order = orderResponse;

        }
        catch (Exception e)
        {
            return NotFound("An error occurred while retrieving the order details.");
        }

        return Page();
    }
    
    
}