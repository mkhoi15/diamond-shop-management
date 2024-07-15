using BusinessObject.Enum;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.PaperworkManagement
{
    public class PaperworkDetailModel : PageModel
    {
        private readonly IPaperworkServices _paperworkService;

        public PaperworkDetailModel(IPaperworkServices paperworkService)
        {
            _paperworkService = paperworkService;
        }

        public PaperworkResponse Paperwork { get; set; }
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? paperworkId, CancellationToken cancellationToken)
        {
            Paperwork = await _paperworkService.GetByIdAsync(paperworkId, default);

            if (Paperwork == null)
            {
                Message = "Paperwork is not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            return Page();
        }
    }
}
