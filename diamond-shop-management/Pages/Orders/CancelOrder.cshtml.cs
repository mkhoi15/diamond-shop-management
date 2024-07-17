using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.Models;
using DTO.DiamondAccessoryDto;
using DTO.OrderDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;


namespace diamond_shop_management.Pages.Orders;


[Authorize(Roles = $"{nameof(Roles.Manager)},{nameof(Roles.User)}")]
public class CancelOrder : PageModel
{
    private readonly IOrderServices _orderServices;
    private readonly IUserServices _userServices;
    private readonly IMapper _mapper;
    private readonly IDiamondAccessoryServices _diamondAccessoryServices;
    private readonly IDiamondServices _diamondServices;
    private readonly IOrderDetailServices _orderDetailServices;

    public CancelOrder(IOrderServices orderServices, IUserServices userServices, IMapper mapper, IDiamondAccessoryServices diamondAccessoryServices, IDiamondServices diamondServices, IOrderDetailServices orderDetailServices)
    {
        _orderServices = orderServices;
        _userServices = userServices;
        _mapper = mapper;
        _diamondAccessoryServices = diamondAccessoryServices;
        _diamondServices = diamondServices;
        _orderDetailServices = orderDetailServices;
    }

    public OrderResponse Order { get; set; }
    
    [BindProperty]
    [Required(ErrorMessage = "Cancellation description is required.")]
    public string CancellationDescription { get; set; }
    
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
            ModelState.AddModelError("", "An error occurred while retrieving the order details.");
            return Page();
        }

        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(Guid orderId, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrEmpty(CancellationDescription))
            {
                ModelState.AddModelError("CancellationDescription", "Cancellation description is required.");
                return Page();
            }

            var orderResponse = await _orderServices.GetOrderByIdAsync(orderId, cancellationToken);
            if (orderResponse == null)
            {
                ModelState.AddModelError("", "Order not found.");
                return Page();
            }
            

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
            
            var isDeleteOrderDetail = await _orderDetailServices.DeleteOrderDetailAsync(orderId, cancellationToken);

            if (!isDeleteOrderDetail)
            {
                ModelState.AddModelError("", "Failed to cancel the order.");
                return Page();
            }

            var order = new Order()
            {
                Id = orderId,
                Status = "Cancelled",
                Description = CancellationDescription
            };
            var success = await _orderServices.UpdateOrderAsync(order);
            if (success == null)
            {
                ModelState.AddModelError("", "Failed to cancel the order.");
                return Page();
            }
            
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _userServices.GetUserByIdAsync(userId);

            if (user.Role == Roles.User.ToString())
            {
                return RedirectToPage("/Orders/SelfOrder");
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