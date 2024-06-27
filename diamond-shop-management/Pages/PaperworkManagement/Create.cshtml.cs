using BusinessObject.Enum;
using BusinessObject.Models;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Abstraction;

namespace diamond_shop_management.Pages.PaperworkManagement
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class CreateModel : PageModel
    {
        private readonly IPaperworkServices _paperworkServices;
        private readonly IDiamondServices _diamondService;

        public CreateModel(IPaperworkServices paperworkServices, IDiamondServices diamondService)
        {
            _paperworkServices = paperworkServices;
            _diamondService = diamondService;
        }

        [BindProperty(SupportsGet = true)]
        public PaperworkRequest PaperworkRequest { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid DiamondId { get; set; }

        public string? Message { get; set; }

        public async Task OnGetAsync(Guid diamondId)
        {
            var diamond = await _diamondService.GetByIdAsync(diamondId, default);

            if (diamond == null)
            {
                Message = "Diamond is not found to create its paperwork";
                ModelState.AddModelError("DiamondId", Message);
            }
            else
            {
                DiamondId = diamondId;
            }

            PaperworkRequest.Type = "warranty";
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (PaperworkRequest.Type?.Trim().ToLower() != "warranty")
                {
                    Message = "Please enter a type Warranty";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                if (PaperworkRequest.ExpirationDate < DateTime.Now)
                {
                    Message = "Expiration date must be greater than current date time";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                PaperworkRequest.CreatedDate = DateTime.Now;
                PaperworkRequest.Status = "Active";
                PaperworkRequest.Type = "warranty";

                await _paperworkServices.AddAsync(PaperworkRequest);

                Message = "Warranty created successfully";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
            catch (Exception ex)
            {
                Message = "Warranty creation failed!\n" + ex.Message;
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
        }
    }
}
