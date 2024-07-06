using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.Models;
using DTO.DiamondDto;
using DTO.Media;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Abstraction;
using System.Drawing.Printing;

namespace diamond_shop_management.Pages.DiamondManagement
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class DiamondDetailModel : PageModel
    {
        private readonly IDiamondServices _diamondService;
        private readonly IPaperworkServices _paperworkService;
        private readonly IMediaServices _mediaService;
        private readonly IMapper _mapper;

        public DiamondDetailModel(IDiamondServices diamondService, IPaperworkServices paperworkService, IMapper mapper, IMediaServices mediaService)
        {
            _diamondService = diamondService;
            _paperworkService = paperworkService;
            _mapper = mapper;
            _mediaService = mediaService;
        }

        public DiamondResponse Diamond { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid diamondId, int? pageNumber, CancellationToken cancellationToken)
        {
            Diamond = await _diamondService.GetByIdAsync(diamondId, default);

            if (Diamond == null)
            {
                Message = "Diamond is not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            return Page();
        }
    }
}
