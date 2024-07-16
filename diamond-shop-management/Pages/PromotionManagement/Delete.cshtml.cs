using BusinessObject.Enum;
using DTO.PromotionDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.PromotionManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]
    public class DeleteModel : PageModel
    {
        private readonly IPromotionServices _promotionServices;

        public DeleteModel(IPromotionServices promotionServices)
        {
            _promotionServices = promotionServices;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Promotion.Id == Guid.Empty)
                {
                    Message = "Promotion id is not found";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                Promotion.IsDeleted = true;
                await _promotionServices.Update(Promotion);

                return RedirectToPage("/PromotionManagement/View");
            }
            catch (Exception ex)
            {
                Message = "Promotion is not deleted\n" + ex.Message;
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
        }
    }
}
