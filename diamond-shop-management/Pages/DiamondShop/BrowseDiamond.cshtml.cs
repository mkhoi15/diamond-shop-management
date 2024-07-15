using AutoMapper;
using BusinessObject.Models;
using DTO;
using DTO.DiamondDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;
using Services.Helpers;
using System.ComponentModel.DataAnnotations;

namespace diamond_shop_management.Pages.DiamondShop;

public class BrowseDiamond : PageModel
{
    private readonly IDiamondServices _diamondServices;
    private readonly IMapper _mapper;

    public BrowseDiamond(IDiamondServices diamondServices, IMapper mapper)
    {
        _diamondServices = diamondServices;
        _mapper = mapper;
        PageSize = 8;
    }

    [BindProperty(SupportsGet = true)]
    public string? Origin { get; set; }
    [BindProperty(SupportsGet = true)]
    public string? Color { get; set; }
    [BindProperty(SupportsGet = true)]
    public string? Cut { get; set; }
    [BindProperty(SupportsGet = true)]
    public string? Clarity { get; set; }
    [BindProperty(SupportsGet = true)]
    public string? Weight { get; set; }
    [BindProperty(SupportsGet = true)]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal? MinPrice { get; set; }
    [BindProperty(SupportsGet = true)]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal? MaxPrice { get; set; }

    public List<DiamondResponse> Diamonds { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    public DTO.Card CartItems { get; set; }
    
    public async Task OnGetAsync(int? pageNumber, CancellationToken cancellationToken)
    {
        PageNumber = pageNumber ?? 1;

        var pagedResult = await _diamondServices.GetAllByConditionAsync(
            d => d.IsSold != true && d.IsDeleted != true && d.Origin != null && d.Color != null && d.Cut != null && d.Clarity != null && d.Weight != null && d.Price != null
            && (string.IsNullOrEmpty(Origin) == true? true : d.Origin.Contains(Origin))
            && (string.IsNullOrEmpty(Color) == true? true : d.Color.Contains(Color))
            && (string.IsNullOrEmpty(Cut) == true? true : d.Cut.Contains(Cut))
            && (string.IsNullOrEmpty(Clarity) == true? true : d.Clarity == Clarity)
            && (string.IsNullOrEmpty(Weight) == true? true : d.Weight == Weight)
            && (MinPrice.HasValue? d.Price >= MinPrice : true)
            && (MaxPrice.HasValue ? d.Price <= MaxPrice : true
            ), PageNumber, PageSize, cancellationToken);

        Diamonds = _mapper.Map<List<DiamondResponse>>(pagedResult.Items);
        TotalItems = pagedResult.TotalItems;
        
        // Get the cart items from the session
        CartItems = HttpContext.Session.GetObjectFromJson<DTO.Card>("Cart") ?? new DTO.Card();
    }

    public async Task<IActionResult> OnPostAddToCart(Guid diamondId, CancellationToken cancellationToken)
    {
        var cart = HttpContext.Session.GetObjectFromJson<DTO.Card>("Cart") ?? new DTO.Card();
        
        if (diamondId != Guid.Empty)
        {
            var diamond = await _diamondServices.GetByIdAsync(diamondId, cancellationToken);
            var diamondResponse = _mapper.Map<DiamondResponse>(diamond);
            
            // if (!CartItems.Diamond.Exists(d => d.Id == diamondResponse.Id))
            // {
            //     cart.Diamond.Add(diamondResponse);
            // }
            cart.Diamond.Add(diamondResponse);
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);
        //var cart2 = HttpContext.Session.GetObjectFromJson<DTO.Card>("Cart") ?? new DTO.Card();

        // Redirect to the cart page
        return RedirectToPage();
    }
}
