using System.Security.Claims;
using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.DiamondAccessoryDto;
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
    private readonly IDiamondAccessoryRepository _diamondAccessoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public Create(IOrderServices orderServices, IDiamondAccessoryRepository diamondAccessoryRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _orderServices = orderServices;
        _diamondAccessoryRepository = diamondAccessoryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    [BindProperty]
    public OrderRequest OrderRequest { get; set; }

    public DTO.Card CartItems { get; private set; }
    
    public decimal Total { get; private set; }

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
        
        List<DiamondAccessory> products = new List<DiamondAccessory>();
        foreach (var productRequest in productsRequest)
        {
            products.Add(_mapper.Map<DiamondAccessory>(productRequest));
        }
        
        _diamondAccessoryRepository.AddRange(products);
        await _unitOfWork.SaveChangeAsync();
        
        OrderRequest = new OrderRequest
        {
            TotalPrice = Total,
            OrderDetails = CartItems.Diamond.Select(d => new OrderDetailRequest
            {
                ProductId = _diamondAccessoryRepository.GetProductByDiamondId(d.Id).Result.Id,
                Price = d.Price ?? 0,
                Quantity = 1
            }).ToList()
        };
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var customerId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        OrderRequest.CustomerId = Guid.Parse((customerId ?? throw new InvalidOperationException()));
        
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
    
    
}