using DTO.AccessoryDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.AccessoryManagement
{
    public class AccessoryDetailModel : PageModel
    {
        private readonly IAccessoryServices _accessoryServices;

        public AccessoryDetailModel(IAccessoryServices accessoryServices)
        {
            _accessoryServices = accessoryServices;
        }

        public AccessoryResponse Accessory { get; set; }
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAync(Guid? accessoryId)
        {
            if (accessoryId == null)
            {
                Message = "AccessoryId not found!";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            Accessory = await _accessoryServices.GetAccessoryByIdAsync(accessoryId.Value, default);
            if (Accessory == null)
            {
                Message = "Accessory not found!";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            return Page();
        }
    }
}
