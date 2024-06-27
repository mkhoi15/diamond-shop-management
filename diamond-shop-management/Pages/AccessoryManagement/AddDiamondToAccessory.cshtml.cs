using BusinessObject.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.AccessoryManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]
    public class AddDiamondToAccessoryModel : PageModel
    {
        private readonly IAccessoryServices _accessoryService;

        public AddDiamondToAccessoryModel(IAccessoryServices accessoryService)
        {
            _accessoryService = accessoryService;
        }

        [BindProperty]
        public Guid AccessoryId { get; set; }

        [BindProperty]
        public Guid DiamondId { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            await _accessoryService.AddDiamondToAccessoryAsync(AccessoryId, DiamondId);
            return RedirectToPage("/ViewAccessory");
        }
    }
}
