using DTO.DiamondDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Abstraction;

namespace diamond_shop_management.Pages.DiamondManagement
{
    public class DiamondDetailModel : PageModel
    {
        private readonly IDiamondServices _diamondService;

        public DiamondDetailModel(IDiamondServices diamondService)
        {
            _diamondService = diamondService;
        }

        public DiamondResponse Diamond { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? diamondId)
        {
            if (diamondId == null)
            {
                return NotFound();
            }

            Diamond = await _diamondService.GetByIdAsync(diamondId.Value, default);

            if (Diamond == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
