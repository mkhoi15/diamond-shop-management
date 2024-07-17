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
            await PopulateSelectListsAsync(cancellationToken);
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                // Re-populate dropdown lists if the model state is invalid
                await PopulateSelectListsAsync(cancellationToken);
                return Page();
            }

            try
            {
                await _diamondAccessoryService.CreateDiamondAccessoryAsync(DiamondAccessory);

                Message = "DiamondAccessory created successfully!";
                ModelState.AddModelError(string.Empty, Message);
                await Task.Delay(3000, cancellationToken);
                return RedirectToPage("/DiamondAccessoryManagement/ViewDiamondAccessory");
            }
            catch (Exception e)
            {
                Message = "Create failed: " + e.Message;
                ModelState.AddModelError(string.Empty, Message);
                await PopulateSelectListsAsync(cancellationToken);
                return Page();
            }
        }

        private async Task PopulateSelectListsAsync(CancellationToken cancellationToken)
        {
            var diamonds = _diamondServices.GetAllAsync(cancellationToken);
            Diamonds = diamonds.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = $"{d.Origin} {d.Color} {d.Cut}"
            }).ToList();

            var accessories = await _accessoryServices.GetActiveAccessoriesAsync(a => a.IsDeleted != true, cancellationToken);
            Accessories = accessories.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();
        }
    }
}
