using AutoMapper;
using BusinessObject.Enum;
using DTO.DiamondDto;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.DiamondShop
{
    public class DetailModel : PageModel
    {
        private readonly IDiamondServices _diamondServices;
        private readonly IPaperworkServices _paperworkServices;
        private readonly IMediaServices _mediaServices;
        private readonly IMapper _mapper;

        public DetailModel(IDiamondServices diamondServices, IPaperworkServices paperworkServices, IMediaServices mediaServices, IMapper mapper)
        {
            _diamondServices = diamondServices;
            _paperworkServices = paperworkServices;
            _mediaServices = mediaServices;
            PageSize = 4;
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
            Diamond = await _diamondServices.GetByIdAsync(diamondId, default);

            if (Diamond == null)
            {
                Message = "Diamond is not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            PageNumber = pageNumber ?? 1;

            var pagedResult = await _paperworkServices.GetAllAsync(
                paper => paper.DiamondId == diamondId && paper.IsDeleted != true, PageNumber, PageSize, cancellationToken);

            Paperworks = _mapper.Map<List<PaperworkResponse>>(pagedResult.Items);
            TotalItems = pagedResult.TotalItems;

            return Page();
        }
    }
}
