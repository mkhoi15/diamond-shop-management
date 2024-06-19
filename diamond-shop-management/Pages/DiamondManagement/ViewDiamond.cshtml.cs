using AutoMapper;
using DTO.DiamondDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Abstraction;

namespace diamond_shop_management.Pages.DiamondManagement
{
    public class ViewDiamondModel : PageModel
    {
        private readonly IDiamondServices _diamondServices;
        private readonly IMapper _mapper;

        public List<DiamondResponse> Diamonds { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public ViewDiamondModel(IDiamondServices diamondServices, IMapper mapper)
        {
            _diamondServices = diamondServices;
            _mapper = mapper;
            PageSize = 10;
        }

        public async Task OnGetAsync(int? pageNumber, CancellationToken cancellationToken)
        {
            PageNumber = pageNumber ?? 1;

            var pagedResult = await _diamondServices.GetAllAsync(PageNumber, PageSize, cancellationToken);

            Diamonds = _mapper.Map<List<DiamondResponse>>(pagedResult.Items);
            TotalItems = pagedResult.TotalItems;
        }
    }
}
