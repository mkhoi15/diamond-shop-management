using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Models;
using Services.Abstraction;
using DTO.DiamondDto;
using DTO.PromotionDto;
using BusinessObject.Enum;
using Microsoft.AspNetCore.Authorization;

namespace diamond_shop_management.Pages.PromotionManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]
    public class ViewModel : PageModel
    {
        private readonly IPromotionServices _promotionService;

        public ViewModel(IPromotionServices promotionService)
        {
            _promotionService = promotionService;
            PageSize = 4;
        }

        public IEnumerable<PromotionResponse> Promotions { get; set; }

        public List<DiamondResponse> Diamonds { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }
        public string? Message { get; set; }

        public async Task OnGetAsync(int? pageNumber, CancellationToken cancellationToken)
        {
            try
            {
                PageNumber = pageNumber ?? 1;

                Promotions = await _promotionService.GetPromotionsByCondition(
                    p => p.IsDeleted != true
                    && (string.IsNullOrEmpty(Search) == true ? true : 
                    p.Name == Search),
                    cancellationToken);
                TotalItems = Promotions.Count();
                Promotions = Promotions
                    .OrderByDescending(Promotion => Promotion.CreateAt)
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize);
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
