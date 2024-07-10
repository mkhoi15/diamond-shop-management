using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Models;
using Services.Abstraction;
using DTO.PromotionDto;
using Microsoft.AspNetCore.Authorization;
using BusinessObject.Enum;

namespace diamond_shop_management.Pages.PromotionManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]
    public class UpdateModel : PageModel
    {
        private readonly IPromotionServices _promotionServices;

        public UpdateModel(IPromotionServices promotionServices)
        {
            _promotionServices = promotionServices;
        }

        [BindProperty]
        public PromotionResponse Promotion { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                Message = "Promotion id is not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            Promotion = await _promotionServices.GetByIdAsync(id.Value, cancellationToken);

            if (Promotion == null)
            {
                Message = "Promotion is not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Promotion.Id == Guid.Empty)
            {
                Message = "Promotion id is not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            if (Promotion.EndDate <= Promotion.StartDate)
            {
                Message = "End date must be greater than start date";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            Promotion.DiscountRate = Double.Parse(Promotion.Name.TrimEnd('%')) / 100.0;
            await _promotionServices.Update(Promotion);

            return RedirectToPage("/PromotionManagement/View");
        }
    }
}
