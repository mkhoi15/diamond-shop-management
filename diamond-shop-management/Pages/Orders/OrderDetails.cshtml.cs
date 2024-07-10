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
            var orderResponse = await _orderServices.GetOrderByIdAsync(orderId, cancellationToken);
            var diamonds = await _orderServices.GetDiamondsByOrderId(orderId, cancellationToken);

            var diamondsId = diamonds.Select(p => p.Id).ToList();

            List<DiamondAccessoryRequest> diamondsRequest = new List<DiamondAccessoryRequest>();
            foreach (var d in diamonds)
            {
                diamondsRequest.Add(new DiamondAccessoryRequest
                {
                    DiamondId = d.Id,
                    CustomerId = orderResponse.CustomerId
                });
            }

            await _diamondAccessoryServices.DeleteProduct(diamondsRequest);

            foreach (var diamond in diamondsId)
            {
                await _diamondServices.UpdateDiamondStatusAsync(diamond, false, cancellationToken);
            }
            
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