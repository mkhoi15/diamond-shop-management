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
    [Authorize(Roles = nameof(Roles.Admin))]
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
            if (Promotion.StartDate < DateTime.Now)
            {
                Message = "Start date must be greater than current date";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }    
            if (Promotion.EndDate <= Promotion.StartDate)
            {
                Message = "End date must be greater than start date";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            await _promotionServices.Add(Promotion);

            Message = "Promotion is added successfully";
            ModelState.AddModelError(string.Empty, Message);

            return Page();
        }
    }
}
