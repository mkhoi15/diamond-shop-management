using AutoMapper;
using BusinessObject.Enum;
using DTO.AccessoryDto;
using DTO.DiamondAccessoryDto;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.AccessoryManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]

    public class ViewAccessoryModel : PageModel
    {
        private readonly IAccessoryServices _accessoryServices;
        private readonly IMapper _mapper;

        public List<AccessoryResponse> Accessories { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public string Message { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchPrice { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ShowOnlyPromotions { get; set; }

        public ViewAccessoryModel(IAccessoryServices accessoryServices, IMapper mapper)
        {
            _accessoryServices = accessoryServices;
            _mapper = mapper;
            PageSize = 5;
        }

        public async Task OnGetAsync(int? pageNumber, CancellationToken cancellationToken)
        {
            try
            {
                PageNumber = pageNumber ?? 1;

                var normalizedSearchPrice = SearchPrice?.Replace(",", "").Trim();

                var pageResult = await _accessoryServices.GetAccessoriesAsync(
                    a => a.IsDeleted != true &&
                         (string.IsNullOrEmpty(Search) || a.Name.Contains(Search)) &&
                         (string.IsNullOrEmpty(SearchPrice) || a.Price.ToString().Contains(normalizedSearchPrice)) &&
                         (!ShowOnlyPromotions || a.PromotionId != null),
                    PageNumber, PageSize, cancellationToken);
                if (pageResult == null)
                {
                    Message = "Failed to retrieve accessories.";
                    return;
                }

                Accessories = _mapper.Map<List<AccessoryResponse>>(pageResult.Items);
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
