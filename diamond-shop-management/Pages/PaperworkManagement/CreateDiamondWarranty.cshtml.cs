using DTO.PaperworkDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.PaperworkManagement;

public class CreateDiamondWarranty : PageModel
{
    private readonly IPaperworkServices _paperworkServices;
    private readonly IDiamondServices _diamondService;

    public CreateDiamondWarranty(IPaperworkServices paperworkServices, IDiamondServices diamondService)
    {
        _paperworkServices = paperworkServices;
        _diamondService = diamondService;
    }
    
    [BindProperty(SupportsGet = true)]
    public PaperworkRequest PaperworkRequest { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid DiamondId { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid OrderId { get; set; }

    public string? Message { get; set; }

    public async Task OnGetAsync(Guid diamondId, Guid orderId)
    {
        var diamond = await _diamondService.GetByIdAsync(diamondId, default);

        if (diamond == null)
        {
            Message = "Diamond not found to create its paperwork";
            ModelState.AddModelError("DiamondId", Message);
        }
        else
        {
            DiamondId = diamondId;
            OrderId = orderId;
            PaperworkRequest = new PaperworkRequest
            {
                DiamondId = diamondId,
                Type = "warranty"
            };
        }
    }
    
    public async Task<IActionResult> OnPost()
    {
        try
        {
            if (PaperworkRequest.ExpirationDate <= DateTime.Now)
            {
                Message = "Expiration date must be greater than current date time";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            PaperworkRequest.CreatedDate = DateTime.Now;
            PaperworkRequest.Status = "Active";
            PaperworkRequest.IsDeleted = false;

            await _paperworkServices.AddAsync(PaperworkRequest);

            return RedirectToPage("/Delivery/CreateDelivery", new { id = OrderId });
        }
        catch (Exception ex)
        {
            Message = "Creation failed: " + ex.Message;
            ModelState.AddModelError(string.Empty, Message);
            return Page();
        }
    }
    
}