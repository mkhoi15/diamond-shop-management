using AutoMapper;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.AccessoryDto;
using DTO.Media;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;
using System.Text.Json;

namespace diamond_shop_management.Pages.AccessoryManagement
{
    /*    [Authorize(Roles = nameof(Roles.Manager))]
    */
    public class UpdateAccessoryModel : PageModel
    {
        private readonly IAccessoryServices _accessoryService;
        private readonly IMediaServices _mediaServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public UpdateAccessoryModel(IAccessoryServices accessoryService, IMediaServices mediaServices, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _accessoryService = accessoryService;
            _mediaServices = mediaServices;
            _unitOfWork = unitOfWork;
            _environment = webHostEnvironment;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public AccessoryResponse Accessory { get; set; }

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid accessoryId)
        {
            try
            {
                var accessory = await _accessoryService.GetAccessoryByIdAsync(accessoryId, default);
                if (accessory == null)
                {
                    Message = "Accessory not found";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                TempData["Media"] = JsonSerializer.Serialize(accessory.Media);

                Accessory = accessory; // Set the Accessory property

                return Page();
            }
            catch (Exception ex)
            {
                Message = "Error fetching accessory: " + ex.Message;
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid accessoryId, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                MediaResponse? media = await SaveMedia(ImageFile);

                Accessory.Id = accessoryId;

                var jsonMedia = TempData["Media"] as string;
                var updatedMedia = JsonSerializer.Deserialize<MediaResponse>(jsonMedia);

                if (media != null)
                {
                    if (updatedMedia != null)
                    {
                        updatedMedia.Url = media.Url;
                        _mediaServices.Update(updatedMedia);
                    }
                    else
                    {
                        var addMedia = _mediaServices.Add(media);
                        Accessory.MediaId = addMedia.Id;
                    }
                }

                _accessoryService.UpdateAccessory(Accessory);
                await _unitOfWork.SaveChangeAsync(cancellationToken);

                Message = "Accessory updated successfully";
                ModelState.AddModelError(string.Empty, Message);

                return RedirectToPage("/AccessoryManagement/ViewAccessory", new { id = Accessory.Id });
            }
            catch (Exception ex)
            {
                Message = "Update failed: " + ex.Message;
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
        }

        private async Task<MediaResponse?> SaveMedia(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            try
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                var relativePath = Path.Combine("images", uniqueFileName).Replace("\\", "/");

                var media = new Media
                {
                    Url = relativePath,
                    IsDeleted = false
                };

                return _mapper.Map<MediaResponse>(media);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save media: " + ex.Message, ex);
            }
        }
    }
}
