using AutoMapper;
using BusinessObject.Enum;
using DTO.DiamondAccessoryDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.DiamondAccessoryManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]
    public class DiamondAccessoryDetailModel : PageModel
    {
        private readonly IDiamondAccessoryServices _diamondAccessoryService;
        private readonly IMapper _mapper;

        public DiamondAccessoryDetailModel(IDiamondAccessoryServices diamondAccessoryService, IMapper mapper)
        {
            _diamondAccessoryService = diamondAccessoryService;
            _mapper = mapper;
        }

        [BindProperty]
        public DiamondAccessoryResponse DiamondAccessory { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
        {
            var diamondAccessory = await _diamondAccessoryService.GetDiamondAccessoryByIdAsync(id, cancellationToken);
            DiamondAccessory = _mapper.Map<DiamondAccessoryResponse>(diamondAccessory);
            return Page();
        }
    }
}
