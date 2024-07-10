using AutoMapper;
using BusinessObject.Enum;
using DTO.DiamondAccessoryDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Abstraction;

namespace diamond_shop_management.Pages.DiamondAccessoryManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]
    public class UpdateDiamondAccessoryModel : PageModel
    {
        private readonly IDiamondAccessoryServices _diamondAccessoryService;
        private readonly IDiamondServices _diamondService;
        private readonly IAccessoryServices _accessoryService;
        private readonly IMapper _mapper;

        public UpdateDiamondAccessoryModel(IDiamondAccessoryServices diamondAccessoryService, IDiamondServices diamondService, IAccessoryServices accessoryService, IMapper mapper)
        {
            _diamondAccessoryService = diamondAccessoryService;
            _diamondService = diamondService;
            _accessoryService = accessoryService;
            _mapper = mapper;
        }

        [BindProperty]
        public DiamondAccessoryRequest DiamondAccessory { get; set; }

        public IEnumerable<SelectListItem> Diamonds { get; set; }
        public IEnumerable<SelectListItem> Accessories { get; set; }
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
        {
            var diamondAccessory = await _diamondAccessoryService.GetDiamondAccessoryByIdAsync(id, cancellationToken);
            if (diamondAccessory == null)
            {
                Message = "Accessory not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            DiamondAccessory = _mapper.Map<DiamondAccessoryRequest>(diamondAccessory);

            var diamonds = _diamondService.GetAllAsync(cancellationToken);
            Diamonds = diamonds.Select(d => new SelectListItem { Value = d.Id.ToString() });

            var accessories = await _accessoryService.GetActiveAccessoriesAsync(a => a.IsDeleted != true,cancellationToken);
            Accessories = accessories.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name });

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            try
            {
                await _diamondAccessoryService.UpdateDiamondAccessoryAsync(id, DiamondAccessory);

                Message = "DiamondAccessory updated successfully";
                ModelState.AddModelError(string.Empty, Message);
                return RedirectToPage("/DiamondAccessoryManagement/ViewDiamondAccessory");
            }
            catch (Exception ex)
            {

                Message = "Update failed: " + ex.Message;
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            
        }
    }
}
