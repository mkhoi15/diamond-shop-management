using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.Models;
using DTO.DiamondAccessoryDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Abstraction;
using System.Linq.Expressions;

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
        public string Message { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }
        public async Task OnGetAsync(int? pageNumber, CancellationToken cancellationToken)
        {
            try
            {
                PageNumber = pageNumber ?? 1;

                Expression<Func<DiamondAccessory, bool>> searchPredicate = da => da.Diamond != null && da.Accessory != null &&(string.IsNullOrEmpty(Search) ||
                da.Diamond.Origin.Contains(Search) ||
                da.Diamond.Color.Contains(Search) ||
                da.Diamond.Cut.Contains(Search) ||
                da.Accessory.Name.Contains(Search));

                var pageResult = await _diamondAccessoryService.GetDiamondAccessoriesAsync(
                    searchPredicate,
                    PageNumber, PageSize, cancellationToken);


                if (pageResult == null)
                {
                    Message = "Failed to retrieve diamond accessories.";
                    return;
                }

                DiamondAccessories = _mapper.Map<List<DiamondAccessoryResponse>>(pageResult.Items);
                TotalItems = pageResult.TotalItems;
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
