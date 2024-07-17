using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.AccessoryDto;
using DTO.Media;
using DTO.PromotionDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Services.Abstraction;
using System.Threading;

namespace diamond_shop_management.Pages.AccessoryManagement
{
    [Authorize(Roles = nameof(Roles.Manager))]

    public class CreateAccessoryModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IAccessoryServices _services;
        private readonly IPromotionServices _promotionServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        [BindProperty]
        public AccessoryRequest AccessoryRequest { get; set; }
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public string? Message { get; set; }
        public CreateAccessoryModel(IWebHostEnvironment environment, IAccessoryServices services, IUnitOfWork unitOfWork, IMapper mapper, IPromotionServices promotionServices)
        {
            _environment = environment;
            _services = services;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _promotionServices = promotionServices;
        }
        public async Task OnGetAsync()
        {
            await PopulateSelectListsAsync();
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            try
            {
                MediaRequest? media = await SaveMedia(ImageFile);

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                if (await _services.AccessoryNameExistsAsync(AccessoryRequest.Name, cancellationToken))
                {
                    Message = "An accessory with this name already exists.";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                if (media != null)
                {
                    AccessoryRequest.Media = media;
                }

                _services.CreateAccessory(AccessoryRequest);
                await _unitOfWork.SaveChangeAsync();

                Message = "Accessory created successfully!";
                ModelState.AddModelError(string.Empty, Message);

                return Page();
            }
            catch (Exception e)
            {
                Message = "Create failed:" + e.Message;
                ModelState.AddModelError(string.Empty, e + Message);
                return Page();

            }
        }

        private async Task<MediaRequest?> SaveMedia(IFormFile imageFile)
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
                IsDeleted = false,
            };

            // Save media to the database
            var mediaRequest = _mapper.Map<MediaRequest>(media);

            // Return the mapped media request
            return mediaRequest;
        }
        private async Task PopulateSelectListsAsync()
        {
            IEnumerable<PromotionResponse> promotions = await _promotionServices.GetPromotionsByCondition(
                    p => p.IsDeleted != true
                    && p.StartDate <= DateTime.Now
                    && p.EndDate >= DateTime.Now, default);
            ViewData["Promotions"] = new SelectList(promotions, "Id", "Name");
        }
    }
}
