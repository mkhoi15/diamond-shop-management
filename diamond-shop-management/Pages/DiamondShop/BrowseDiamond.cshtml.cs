using AutoMapper;
using BusinessObject.Models;
using DTO.DiamondDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;
using Services.Helpers;

namespace diamond_shop_management.Pages.DiamondShop;

public class BrowseDiamond : PageModel
{
    private readonly IDiamondServices _diamondServices;
    private readonly IMapper _mapper;

    public BrowseDiamond(IDiamondServices diamondServices, IMapper mapper)
    {
        _diamondServices = diamondServices;
        _mapper = mapper;
        PageSize = 10;
    }

    public List<DiamondResponse> Diamonds { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    public DTO.Card CartItems { get; set; }
    
    public async Task OnGetAsync(int? pageNumber, CancellationToken cancellationToken)
    {
        PageNumber = pageNumber ?? 1;

        var pagedResult = await _diamondServices.GetAllByConditionAsync(d => d.IsSold != true , PageNumber, PageSize, cancellationToken);

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
