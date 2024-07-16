using DTO.DiamondDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IDiamondServices _diamondServices;

    public List<DiamondResponse> Diamonds { get; set; }
    
    public IndexModel(ILogger<IndexModel> logger, IDiamondServices diamondServices)
    {
        _logger = logger;
        _diamondServices = diamondServices;
    }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        Diamonds = (await _diamondServices.GetAllByConditionAsync(d => d.IsDeleted != true && d.IsSold != true, cancellationToken))
            .Take(8)
            .ToList();
    }
}