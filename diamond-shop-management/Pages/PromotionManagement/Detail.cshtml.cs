using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessObject.Models;
using Services.Abstraction;
using DTO.PromotionDto;
using BusinessObject.Enum;
using Microsoft.AspNetCore.Authorization;

namespace diamond_shop_management.Pages.PromotionManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]
    public class DetailModel : PageModel
    {
        private readonly IPromotionServices _promotionServices;

        public DetailModel(IPromotionServices promotionServices)
        {
            _promotionServices = promotionServices;
        }

        public PromotionResponse Promotion { get; set; }

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
                return Page();
            }
            catch (Exception ex)
            {
                Message = "Promotion is not found\n" + ex.Message;
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
        }
    }
}
