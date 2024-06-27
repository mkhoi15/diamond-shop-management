using AutoMapper;
using DTO.DiamondDto;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.DiamondManagement
{
    public class UpdateModel : PageModel
    {
        private readonly IDiamondServices _diamondService;
        private readonly IPaperworkServices _paperworkService;
        private readonly IMapper _mapper;

        public UpdateModel(IDiamondServices diamondService, IPaperworkServices paperworkService, IMapper mapper)
        {
            _diamondService = diamondService;
            _paperworkService = paperworkService;
            PageSize = 8;
            _mapper = mapper;
        }

        public DiamondResponse Diamond { get; set; }

        public List<PaperworkResponse> Paperworks { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

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

            PageNumber = pageNumber ?? 1;

            var pagedResult = await _paperworkService.GetAllAsync(
                paper => paper.DiamondId == diamondId && paper.Status == "Active", PageNumber, PageSize, cancellationToken);

            Paperworks = _mapper.Map<List<PaperworkResponse>>(pagedResult.Items);

            return Page();
        }
    }
}
