using AutoMapper;
using BusinessObject.Enum;
using DataAccessLayer.Abstraction;
using DTO.AccessoryDto;
using DTO.Media;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;
using System.Text.Json;

namespace diamond_shop_management.Pages.AccessoryManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]

    public class DeleteAccessoryModel : PageModel
    {
        private readonly IAccessoryServices _accessoryServices;
        private readonly IMediaServices _mediaServices;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAccessoryModel(IAccessoryServices accessoryServices, IMediaServices mediaServices, IUnitOfWork unitOfWork)
        {
            _accessoryServices = accessoryServices;
            _mediaServices = mediaServices;
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)]
        public AccessoryResponse Accessory { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid AccessoryId { get; set; }
        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid accessoryId)
        {
            Accessory = await _accessoryServices.GetAccessoryByIdAsync(accessoryId, default);

            if (Accessory == null)
            {
                Message = "Accessory not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            TempData["Media"] = JsonSerializer.Serialize(Accessory.Media);

            return Page();

        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            try
            {
                var jsonMedia = TempData["Media"] as string;
                var media = JsonSerializer.Deserialize<MediaResponse>(jsonMedia);
                if (media != null)
                {
                    media.IsDeleted = true;
                    _mediaServices.Update(media);
                }

                var success = await _accessoryServices.DeleteAccessory(AccessoryId);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                

                Message = "Accessory deleted successfully";
                ModelState.AddModelError(string.Empty, Message);
                return RedirectToPage("/AccessoryManagement/ViewAccessory");
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
