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
    
    
    public async Task OnGetAsync(int? pageNumber, CancellationToken cancellationToken)
    {
        PageNumber = pageNumber ?? 1;

        var pagedResult = await _diamondServices.GetAllAsync(PageNumber, PageSize, cancellationToken);

        Diamonds = _mapper.Map<List<DiamondResponse>>(pagedResult.Items);
        TotalItems = pagedResult.TotalItems;
    }

    public IActionResult OnPostAddToCart(Guid diamondId, CancellationToken cancellationToken)
    {
        var cart = HttpContext.Session.GetObjectFromJson<List<DiamondAccessory>>("Cart") ?? new List<DiamondAccessory>();

        if (diamondId != Guid.Empty)
        {
            var diamond = _diamondServices.GetByIdAsync(diamondId, cancellationToken).Result;
            var item = new DiamondAccessory
            {
                Id = Guid.NewGuid(),
                DiamondId = diamondId,
                Diamond = _mapper.Map<Diamond>(diamond)
            };
            cart.Add(item);
        }

        HttpContext.Session.SetObjectAsJson("Cart", cart);

        // Redirect to the cart page
        return RedirectToPage("../Card/Card");
    }
}
