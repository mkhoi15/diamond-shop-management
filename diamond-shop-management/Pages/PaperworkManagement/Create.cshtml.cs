using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.Models;
using DTO.Media;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Abstraction;

namespace diamond_shop_management.Pages.PaperworkManagement
{
    [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Manager)}")]
    public class CreateModel : PageModel
    {
        private readonly IPaperworkServices _paperworkServices;
        private readonly IWebHostEnvironment _environment;
        private readonly IDiamondServices _diamondService;
        private readonly IMapper _mapper;

        public CreateModel(IPaperworkServices paperworkServices, IDiamondServices diamondService, IWebHostEnvironment environment, IMapper mapper)
        {
            _paperworkServices = paperworkServices;
            _diamondService = diamondService;
            _environment = environment;
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
            try
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
            catch (Exception ex)
            {
                Message = "Created failed!\n" + ex.Message;
                ModelState.AddModelError(string.Empty, Message);
            }
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                MediaRequest? paperMedia = await SaveMedia(PaperFile);

                if (paperMedia is not null)
                {
                    PaperworkRequest.Media = paperMedia;
                }
                PaperworkRequest.CreatedDate = DateTime.Now;
                PaperworkRequest.Status = "Active";
                PaperworkRequest.IsDeleted = false;

                await _paperworkServices.AddAsync(PaperworkRequest);

                return RedirectToPage("/DiamondManagement/Update", new { DiamondId = DiamondId });
            }
            catch (Exception ex)
            {
                Message = "creation failed!\n" + ex.Message;
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
