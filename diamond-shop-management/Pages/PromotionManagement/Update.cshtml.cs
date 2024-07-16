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
using System.ComponentModel.DataAnnotations;
using System.Threading;

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

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDateValue { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id, CancellationToken cancellationToken)
        {
            try
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

                if (Promotion.StartDate != null)
                    StartDateValue = Promotion.StartDate.Value;

                return Page();
            }
            catch (Exception ex)
            {
                Message = "Promotion is not found\n" + ex.Message;
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (Promotion.Id == Guid.Empty)
                {
                    Message = "Promotion id is not found";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                if (Promotion.EndDate != null && Promotion.StartDate != null && Promotion.EndDate.Value.Date <= Promotion.StartDate.Value.Date)
                {
                    Message = "End date must be greater than start date";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                Promotion.DiscountRate = Double.Parse(Promotion.Name.TrimEnd('%')) / 100.0;

                var ExistedPromotion = await _promotionServices
                    .GetPromotionsByCondition(p => p.DiscountRate == Promotion.DiscountRate
                    && p.IsDeleted != true
                    && p.Id != Promotion.Id, cancellationToken);

                if (ExistedPromotion.Any())
                {
                    Message = "Promotion name is already existed";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }
                await _promotionServices.Update(Promotion);

                return RedirectToPage("/PromotionManagement/View");
            }
            catch (Exception ex)
            {
                Message = "Promotion is not updated\n" + ex.Message;
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
        }
    }
}
