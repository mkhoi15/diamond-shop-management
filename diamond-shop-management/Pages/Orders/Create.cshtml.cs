using System.Security.Claims;
using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.DiamondAccessoryDto;
using DTO.Enum;
using DTO.OrderDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repositories.Abstraction;
using Services.Abstraction;
using Services.Helpers;

namespace diamond_shop_management.Pages.Orders;

public class Create : PageModel
{
    private readonly IOrderServices _orderServices;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IDiamondAccessoryServices _diamondAccessoryServices;

    public Create(IOrderServices orderServices, IUnitOfWork unitOfWork, IMapper mapper, IDiamondAccessoryServices diamondAccessoryServices)
    {
        _orderServices = orderServices;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _diamondAccessoryServices = diamondAccessoryServices;
    }


    [BindProperty]
    public OrderRequest OrderRequest { get; set; }

    public DTO.Card CartItems { get; private set; }
    
    public decimal Total { get; private set; }
    
    public List<Guid> ProductIds { get; set; } = new List<Guid>();

    public async Task OnGet()
    {
        
        CartItems = HttpContext.Session.GetObjectFromJson<DTO.Card>("Cart") ?? new DTO.Card();
        this.TotalPrice();

        List<DiamondAccessoryRequest> productsRequest = new List<DiamondAccessoryRequest>();
        foreach (var diamonds in CartItems.Diamond)
        {
            productsRequest.Add(new DiamondAccessoryRequest
            {
                DiamondId = diamonds.Id,
            });
        }

        await _diamondAccessoryServices.AddProducts(productsRequest);
        
        ProductIds = CartItems.Diamond.Select(d => _diamondAccessoryServices.GetProductByDiamondId(d.Id).Result.Id).ToList();
        
        OrderRequest = new OrderRequest
        {
            TotalPrice = Total,
            OrderDetails = CartItems.Diamond.Select(d => new OrderDetailRequest
            {
                ProductId = _diamondAccessoryServices.GetProductByDiamondId(d.Id).Result.Id,
                Price = d.Price ?? 0,
                Quantity = 1
            }).ToList()
        };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        OrderRequest.CustomerId = Guid.Parse((customerId ?? throw new InvalidOperationException()));

        OrderRequest.Status = "Pending";
        
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        
        var result = await _orderServices.CreateOrderAsync(OrderRequest);
    
        if (result is not null)
        {
            // Clear the cart after successful order creation
            HttpContext.Session.Remove("Cart");
            return RedirectToPage("/Orders/Success");
        }
    
        ModelState.AddModelError("", "There was an error creating the order. Please try again.");
        return Page();
    }
    
    public void TotalPrice()
    {
        decimal total = 0;
        if (this.CartItems.Diamond.Count > 0)
        {
            foreach (var diamond in this.CartItems.Diamond)
            {
                if (diamond.Price != null)
                {
                    total = total + decimal.Parse(diamond.Price.ToString()!);
                }
            }
        }

        this.Total = total;

    }

    public async Task<IActionResult> OnPostCancelBookingAsync()
    {
        if (ProductIds.Count > 0)
        {
            foreach (var productId in ProductIds)
            {
                await _diamondAccessoryServices.DeleteDiamondAccessory(productId);
            }
        }
        
        return RedirectToPage("/Card/Card");
        
    }
    
}