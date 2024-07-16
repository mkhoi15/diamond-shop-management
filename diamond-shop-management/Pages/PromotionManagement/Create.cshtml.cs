using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Models;
using Services.Abstraction;
using DTO.PromotionDto;
using DataAccessLayer.Abstraction;
using BusinessObject.Enum;
using Microsoft.AspNetCore.Authorization;

namespace diamond_shop_management.Pages.PromotionManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]
    public class CreateModel : PageModel
    {
        private readonly IPromotionServices _promotionServices;

        public CreateModel(IPromotionServices promotionServices)
        {
            _promotionServices = promotionServices;
        }

        [BindProperty]
        public PromotionRequest Promotion { get; set; }

        public string? Message { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (Promotion.StartDate != null && Promotion.StartDate.Value.Date < DateTime.Now.Date)
                {
                    Message = "Start date must be greater than current date";
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
                    && p.IsDeleted != true, cancellationToken);

                if (ExistedPromotion.Any())
                {
                    Message = "Promotion name is already existed";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }
                Promotion.CreateAt = DateTime.Now;
                await _promotionServices.Add(Promotion);

                /*Message = "Promotion is added successfully";
                ModelState.AddModelError(string.Empty, Message);*/

                return RedirectToPage("/PromotionManagement/View");
            }
            catch (Exception ex)
            {
                Message = "Promotion is not added\n" + ex.Message;
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
        }
    }
}
