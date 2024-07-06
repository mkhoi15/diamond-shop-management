using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.Media;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Abstraction;
using System.Text.Json;

namespace diamond_shop_management.Pages.DiamondManagement
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class UpdateCertificateModel : PageModel
    {
        private readonly IPaperworkServices _paperworkServices;
        private readonly IMediaServices _mediaServices;
        private readonly IWebHostEnvironment _environment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCertificateModel(IPaperworkServices paperworkServices, IWebHostEnvironment environment, IMapper mapper, IUnitOfWork unitOfWork, IMediaServices mediaServices)
        {
            _paperworkServices = paperworkServices;
            _environment = environment;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _mediaServices = mediaServices;
        }

        [BindProperty(SupportsGet = true)]
        public PaperworkResponse Paperwork { get; set; }

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? paperworkId, CancellationToken cancellationToken)
        {
            Paperwork = await _paperworkServices.GetByIdAsync(paperworkId, default);

            if (Paperwork == null)
            {
                Message = "Paperwork is not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            TempData["Media"] = JsonSerializer.Serialize(Paperwork.Media);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (Paperwork.ExpirationDate != null && Paperwork.ExpirationDate <= Paperwork.CreatedDate)
                {
                    Message = "Expiration date must be greater than created date";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                MediaResponse? media = await SaveMedia(ImageFile);

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
                        Paperwork.MediaId = addMedia.Id;
                    }
                }
                _paperworkServices.Update(Paperwork);
                await _unitOfWork.SaveChangeAsync(cancellationToken);

                Message = "Paperwork is updated successfully";
                ModelState.AddModelError(string.Empty, Message);

                return RedirectToPage("/DiamondManagement/UpdateCertificate", Paperwork.Id);
            }
            catch (Exception ex)
            {
                Message = "Update failed!\n" + ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);

                return Page();
            }
        }

        private async Task<MediaResponse?> SaveMedia(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

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
    }
}
