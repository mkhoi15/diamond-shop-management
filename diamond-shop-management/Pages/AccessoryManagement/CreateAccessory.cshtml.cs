using DTO.AccessoryDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.AccessoryManagement
{
    public class CreateAccessoryModel : PageModel
    {
        private readonly IAccessoryServices _services;

        [BindProperty]
        public AccessoryRequest AccessoryRequest { get; set; }
        public string? Message { get; set; }
        public CreateAccessoryModel(IAccessoryServices services)
        {
            _services = services;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                await _services.CreateAccessoryAsync(AccessoryRequest);

                Message = "Accessory Create successfully!";
                return Page();
            }
            catch (Exception e)
            {
                Message = "Create failed\n" + e.Message;
                ModelState.AddModelError(string.Empty, e + Message);
                return Page();

            }
        }
    }
}
