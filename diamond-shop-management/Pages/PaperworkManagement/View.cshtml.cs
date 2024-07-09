using AutoMapper;
using BusinessObject.Enum;
using DTO.DiamondDto;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Abstraction;

namespace diamond_shop_management.Pages.PaperworkManagement
{
    [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Manager)}")]
    public class ViewModel : PageModel
    {
        private readonly IPaperworkServices _paperworkService;
        private readonly IDiamondServices _diamondService;
        private readonly IMapper _mapper;

        public ViewModel(IPaperworkServices paperworkService, IMapper mapper, IDiamondServices diamondService)
        {
            _paperworkService = paperworkService;
            _mapper = mapper;
            PageSize = 8;
            _diamondService = diamondService;
        }

        public DiamondResponse Diamond { get; set; }
        public List<PaperworkResponse> Paperworks { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public string? Message { get; set; }

        public async Task OnGetAsync(Guid diamondId, int? pageNumber, CancellationToken cancellationToken)
        {
            PageNumber = pageNumber ?? 1;

            Diamond = await _diamondService.GetByIdAsync(diamondId, default);

            if (Diamond == null)
            {
                Message = "Diamond is not found";
                ModelState.AddModelError(string.Empty, Message);
                return;
            }

            var pagedResult = await _paperworkService.GetAllAsync(
                paper => paper.DiamondId == diamondId && paper.Status == "Active", PageNumber, PageSize,cancellationToken);

            Paperworks = _mapper.Map<List<PaperworkResponse>>(pagedResult.Items);

            TotalItems = pagedResult.TotalItems;
        }
    }
}
