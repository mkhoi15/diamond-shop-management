using AutoMapper;
using BusinessObject.Enum;
using DTO.DiamondAccessoryDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Services.Abstraction;

namespace diamond_shop_management.Pages.DiamondAccessoryManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]
    public class CreateDiamondAccessoryModel : PageModel
    {
        private readonly IDiamondAccessoryServices _diamondAccessoryService;
        private readonly IDiamondServices _diamondServices;
        private readonly IAccessoryServices _accessoryServices;
        private readonly IMapper _mapper;

        [BindProperty]
        public DiamondAccessoryRequest DiamondAccessory { get; set; }
        public IEnumerable<SelectListItem> Diamonds { get; set; }
        public IEnumerable<SelectListItem> Accessories { get; set; }
        public string? Message { get; set; }

        public CreateDiamondAccessoryModel(IDiamondAccessoryServices diamondAccessoryService, IMapper mapper, IDiamondServices diamondServices, IAccessoryServices accessoryServices)
        {
            _diamondAccessoryService = diamondAccessoryService;
            _mapper = mapper;
            _diamondServices = diamondServices;
            _accessoryServices = accessoryServices;
        }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {

            var request = _mapper.Map<DiamondAccessoryRequest>(DiamondAccessory);
            var accessories = await _accessoryServices.GetActiveAccessoriesAsync(a => a.IsDeleted != true, cancellationToken);
            Accessories = accessories.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name });
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _diamondAccessoryService.CreateDiamondAccessoryAsync(DiamondAccessory);

                Message = "DiamondAccessory Create successfully!";
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
