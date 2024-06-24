using BusinessObject.Enum;
using DTO.DiamondDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Abstraction;

namespace diamond_shop_management.Pages.DiamondManagement
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class DiamondDetailModel : PageModel
    {
        private readonly IDiamondServices _diamondService;

        public DiamondDetailModel(IDiamondServices diamondService)
        {
            _diamondService = diamondService;
        }

        public DiamondResponse Diamond { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? diamondId)
        {
            if (diamondId == null)
            {
                Message = "Diamond Id is not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            Diamond = await _diamondService.GetByIdAsync(diamondId.Value, default);

            if (Diamond == null)
            {
                Message = "Diamond is not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            return Page();
        }
    }
}
