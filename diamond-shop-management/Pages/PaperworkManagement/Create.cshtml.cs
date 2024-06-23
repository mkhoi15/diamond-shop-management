using BusinessObject.Models;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Abstraction;

namespace diamond_shop_management.Pages.PaperworkManagement
{
    public class CreateModel : PageModel
    {
        private readonly IPaperworkServices _paperworkServices;
        private readonly IDiamondServices _diamondService;

        public CreateModel(IPaperworkServices paperworkServices, IDiamondServices diamondService)
        {
            _paperworkServices = paperworkServices;
            _diamondService = diamondService;
        }

        [BindProperty]
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
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (PaperworkRequest.Type?.Trim().ToLower() != "certificate"
                    && PaperworkRequest.Type?.Trim().ToLower() != "warranty")
                {
                    Message = "Please enter a type Certificate or Warranty";
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
                if (PaperworkRequest.Type.Trim().ToLower() == "certificate")
                {
                    PaperworkRequest.Type = "certificate";
                }
                else
                {
                    PaperworkRequest.Type = "warranty";
                }

                await _paperworkServices.AddAsync(PaperworkRequest);

                Message = "Paperwork created successfully";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
            catch (Exception ex)
            {
                Message = "Paperwork creation failed!\n" + ex.Message;
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
        }
    }
}
