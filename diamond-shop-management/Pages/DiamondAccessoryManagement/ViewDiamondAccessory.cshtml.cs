using AutoMapper;
using BusinessObject.Enum;
using DTO.DiamondAccessoryDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.DiamondAccessoryManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]
    public class ViewDiamondAccessoryModel : PageModel
    {
        private readonly IDiamondAccessoryServices _diamondAccessoryService;
        private readonly IMapper _mapper;

        public ViewDiamondAccessoryModel(IDiamondAccessoryServices diamondAccessoryService, IMapper mapper)
        {
            _diamondAccessoryService = diamondAccessoryService;
            _mapper = mapper;
            PageSize = 5;
        }

        public List<DiamondAccessoryResponse> DiamondAccessories { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public async Task OnGetAsync(int? pageNumber, CancellationToken cancellationToken)
        {
            PageNumber = pageNumber ?? 1;

            var pageResult = await _diamondAccessoryService.GetDiamondAccessoriesAsync(PageNumber, PageSize, cancellationToken);

            DiamondAccessories = _mapper.Map<List<DiamondAccessoryResponse>>(pageResult.Items);
            TotalItems = pageResult.TotalItems;
        }
    }
}
