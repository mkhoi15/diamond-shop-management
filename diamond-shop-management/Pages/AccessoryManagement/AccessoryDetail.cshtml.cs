using BusinessObject.Enum;
using DTO.AccessoryDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.AccessoryManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]

    public class AccessoryDetailModel : PageModel
    {
        private readonly IAccessoryServices _accessoryServices;

        public AccessoryDetailModel(IAccessoryServices accessoryServices)
        {
            _accessoryServices = accessoryServices;
        }

        public AccessoryResponse Accessory { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid accessoryId)
        {
            try
            {
                Accessory = await _accessoryServices.GetAccessoryByIdAsync(accessoryId, default);
                if (Accessory == null)
                {
                    Message = "Accessory not found!";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                return Page();
            }
            catch (Exception ex)
            {
                Message = $"Error fetching accessory: {ex.Message}";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
        }
    }
}
