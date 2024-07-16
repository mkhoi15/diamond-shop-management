using AutoMapper;
using BusinessObject.Enum;
using DataAccessLayer.Abstraction;
using DTO.DiamondAccessoryDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;
using System.Threading;

namespace diamond_shop_management.Pages.DiamondAccessoryManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]
    public class DeleteDiamondAccessoryModel : PageModel
    {
        private readonly IDiamondAccessoryServices _diamondAccessoryService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;


        public DeleteDiamondAccessoryModel(IDiamondAccessoryServices diamondAccessoryService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _diamondAccessoryService = diamondAccessoryService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public DiamondAccessoryResponse DiamondAccessory { get; set; }
        public string? Message { get; set; }


        public async Task<IActionResult> OnGetAsync(Guid id, CancellationToken cancellationToken)
        {
            var diamondAccessory = await _diamondAccessoryService.GetDiamondAccessoryByIdAsync(id, cancellationToken);
            if (diamondAccessory == null)
            {
                Message = "Accessory not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            DiamondAccessory = _mapper.Map<DiamondAccessoryResponse>(diamondAccessory);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _diamondAccessoryService.DeleteDiamondAccessoryAsync(id);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                Message = "Accessory deleted successfully";
                ModelState.AddModelError(string.Empty, Message);
                await Task.Delay(3000, cancellationToken);
                return RedirectToPage("/DiamondAccessoryManagement/ViewDiamondAccessory");
            }
            catch (Exception ex)
            {
                Message = "Delete failed!\n" + ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);

                return Page();
            }
        }
    }
}
