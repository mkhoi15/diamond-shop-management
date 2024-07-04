using AutoMapper;
using BusinessObject.Models;
using DTO.Media;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.PaperworkManagement
{
    public class CreateWarrantyModel : PageModel
    {
        private readonly IPaperworkServices _paperworkServices;
        private readonly IWebHostEnvironment _environment;
        private readonly IDiamondServices _diamondService;
        private readonly IMapper _mapper;

        public CreateWarrantyModel(IPaperworkServices paperworkServices, IWebHostEnvironment environment, IDiamondServices diamondService, IMapper mapper)
        {
            _paperworkServices = paperworkServices;
            _environment = environment;
            _diamondService = diamondService;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public PaperworkRequest PaperworkRequest { get; set; }

        [BindProperty]
        public IFormFile? PaperFile { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid DiamondId { get; set; }

        public string? Message { get; set; }

        public async Task OnGetAsync(Guid diamondId)
        {
            var diamond = await _diamondService.GetByIdAsync(diamondId, default);

            if (diamond == null)
            {
                Message = "Diamond is not found to create its paperwork";
                ModelState.AddModelError("DiamondId", Message);
            }
            else
            {
                DiamondId = diamondId;
            }

            PaperworkRequest.Type = "warranty";
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (PaperworkRequest.Type?.Trim().ToLower() != "warranty")
                {
                    Message = "Please enter a type Warranty";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                if (PaperworkRequest.ExpirationDate < DateTime.Now)
                {
                    Message = "Expiration date must be greater than current date time";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                PaperworkRequest.CreatedDate = DateTime.Now;
                PaperworkRequest.Status = "Active";
                PaperworkRequest.Type = "warranty";

                await _paperworkServices.AddAsync(PaperworkRequest);

                Message = "Warranty created successfully";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }
            catch (Exception ex)
            {
                Message = "Warranty creation failed!\n" + ex.Message;
                ModelState.AddModelError(string.Empty, Message);
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

            return _mapper.Map<MediaRequest>(media);
        }
    }
}
