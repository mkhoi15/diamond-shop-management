using AutoMapper;
using BusinessObject.Enum;
using DTO.DiamondDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Abstraction;

namespace diamond_shop_management.Pages.DiamondManagement
{
    [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Manager)}")]
    public class ViewDiamondModel : PageModel
    {
        private readonly IDiamondServices _diamondServices;
        private readonly IMapper _mapper;

        public List<DiamondResponse> Diamonds { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public string? Message { get; set; }

        public ViewDiamondModel(IDiamondServices diamondServices, IMapper mapper)
        {
            _diamondServices = diamondServices;
            _mapper = mapper;
            PageSize = 8;
        }

        public async Task OnGetAsync(int? pageNumber, CancellationToken cancellationToken)
        {
            try
            {
                PageNumber = pageNumber ?? 1;

                var pagedResult = await _diamondServices.GetAllByConditionAsync(
                    d => d.IsDeleted != true
                    && 
                    (string.IsNullOrEmpty(Search) == true ? true :
                    d.Origin!.Contains(Search!)
                    || d.Color!.Contains(Search!)
                    || d.Cut!.Contains(Search!)
                    || d.Clarity == Search
                    || d.Weight == Search
                    || d.Price.ToString() == Search
                    || d.Promotion!.Name == Search
                    || d.IsSold!.ToString()!.Contains(Search!)), PageNumber, PageSize, cancellationToken);

                Diamonds = _mapper.Map<List<DiamondResponse>>(pagedResult.Items);
                TotalItems = pagedResult.TotalItems;
            }
            catch (Exception e)
            {
                Message = e.Message;
                ModelState.AddModelError(string.Empty, e.Message);
                return;
            }
        }
    }
}
