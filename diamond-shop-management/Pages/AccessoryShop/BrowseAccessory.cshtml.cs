using AutoMapper;
using DTO;
using DTO.AccessoryDto;
using DTO.DiamondDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;
using Services.Helpers;

namespace diamond_shop_management.Pages.AccessoryShop
{
    public class BrowseAccessoryModel : PageModel
    {
        private readonly IAccessoryServices _accessoryServices;
        private readonly IMapper _mapper;

        public BrowseAccessoryModel(IAccessoryServices accessoryServices, IMapper mapper)
        {
            _accessoryServices = accessoryServices;
            _mapper = mapper;
            PageSize = 10;
        }

        public List<AccessoryResponse> Accessories { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public DTO.Card CartItems { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Search { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchPrice { get; set; }
        [BindProperty(SupportsGet = true)]
        public bool ShowOnlyPromotions { get; set; }

        public async Task OnGetAsync(int? pageNumber, CancellationToken cancellationToken)
        {
            PageNumber = pageNumber ?? 1;

            var normalizedSearchPrice = SearchPrice?.Replace(",", "").Trim();
            var pagedResult = await _accessoryServices.GetAccessoryShopAsync(a => a.IsDeleted != true &&
                         (string.IsNullOrEmpty(Search) || a.Name.Contains(Search)) &&
                         (string.IsNullOrEmpty(SearchPrice) || a.Price.ToString().Contains(normalizedSearchPrice)) &&
                         (!ShowOnlyPromotions || a.PromotionId != null),
                         PageNumber, PageSize, cancellationToken);

            Accessories = _mapper.Map<List<AccessoryResponse>>(pagedResult.Items);
            TotalItems = pagedResult.TotalItems;

            // Get the cart items from the session
            CartItems = HttpContext.Session.GetObjectFromJson<DTO.Card>("Cart") ?? new DTO.Card();
        }

        public async Task<IActionResult> OnPostAddToCart(Guid accessoryId, CancellationToken cancellationToken)
        {
            var cart = HttpContext.Session.GetObjectFromJson<DTO.Card>("Cart") ?? new DTO.Card();

            if (accessoryId != Guid.Empty)
            {
                var accessory = await _accessoryServices.GetAccessoryByIdAsync(accessoryId, cancellationToken);
                var accessoryResponse = _mapper.Map<AccessoryResponse>(accessory);

                // Check if the accessory already exists in the cart
                var existingAccessory = cart.Accessories.FirstOrDefault(a => a.Accessory.Id == accessoryResponse.Id);

                if (existingAccessory != null)
                {
                    existingAccessory.Quantity++;
                }
                else
                {
                    cart.Accessories.Add(new AccessoryCard { Accessory = accessoryResponse, Quantity = 1 });
                }

                // Save the cart back to the session
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToPage();
        }
    }
}
