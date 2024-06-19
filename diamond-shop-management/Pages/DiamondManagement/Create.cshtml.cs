using AutoMapper;
using BusinessObject.Models;
using DTO.DiamondDto;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.DiamondManagement
{
    public class CreateModel : PageModel
    {
        private readonly IDiamondServices _diamondServices;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public DiamondRequest DiamondRequest { get; set; }

        public CreateModel(IDiamondServices diamondServices, IMapper mapper)
        {
            _diamondServices = diamondServices;
            _mapper = mapper;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _diamondServices.CreateDiamondAsync(DiamondRequest);

            return RedirectToPage("/DiamondManagement/ViewDiamond");
        }
    }
}
