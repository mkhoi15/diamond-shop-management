using BusinessObject.Enum;
using DTO.DiamondAccessoryDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.DiamondAccessoryManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]

    public class DiamondAccessoryDetailModel : PageModel
    {

        private readonly IDiamondAccessoryServices _diamondAccessoryService;

        public DiamondAccessoryDetailModel(IDiamondAccessoryServices diamondAccessoryService)
        {
            _diamondAccessoryService = diamondAccessoryService;
        }

        [BindProperty]
        public DiamondAccessoryResponse DiamondAccessory { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                DiamondAccessory = await _diamondAccessoryService.GetDiamondAccessoryByIdAsync(id, cancellationToken);

                if (DiamondAccessory == null)
                {
                    ErrorMessage = "Diamond Accessory not found.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred while retrieving the details: {ex.Message}";
            }

            return Page();
        }
    }
}
