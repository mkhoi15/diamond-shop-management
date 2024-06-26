using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.Models;
using DTO.DiamondDto;
using DTO.Media;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;
using System.ComponentModel.DataAnnotations;

namespace diamond_shop_management.Pages.DiamondManagement
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class CreateModel : PageModel
    {
        private readonly IDiamondServices _diamondServices;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        [BindProperty]
        public DiamondRequest DiamondRequest { get; set; }

        [BindProperty]
        public PaperworkRequest PaperworkRequest { get; set; }

        public List<string> Countries { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Cuts { get; set; }

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        [BindProperty]
        public IFormFile? PaperFile { get; set; }

        public string? Message { get; set; }

        public CreateModel(IDiamondServices diamondServices, IPaperworkServices paperworkServices, IWebHostEnvironment environment, IMapper mapper)
        {
            _diamondServices = diamondServices;
            _environment = environment;
            _mapper = mapper;
        }

        public void OnGet()
        {
            InitializeDiamondInfo();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                MediaRequest? media = await SaveMedia(ImageFile);
                MediaRequest? paperMedia = await SaveMedia(PaperFile);

                PaperworkRequest diamondCertificate = new PaperworkRequest
                {
                    Type = "certificate",
                    CreatedDate = DateTime.Now,
                    Status = "Active"
                };
                if (paperMedia is not null)
                {
                    diamondCertificate.Media = paperMedia;
                }
                DiamondRequest.PaperWorks.Add(diamondCertificate);


                if (media is not null)
                {
                    DiamondRequest.Media = media;
                }
                DiamondRequest.CreatedAt = DateTime.Now;

                await _diamondServices.CreateDiamondAsync(DiamondRequest);

                Message = "Create successfully!";
                ModelState.AddModelError(string.Empty, "Create successfully!");

                InitializeDiamondInfo();
                return Page();
            }
            catch (Exception ex)
            {
                Message = "Create failed!\n" + ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);

                InitializeDiamondInfo();
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
            };

            return _mapper.Map<MediaRequest>(media);
        }

        public void InitializeDiamondInfo()
        {
            Countries = new List<string>
            {
                "USA", "India", "South Africa", "Canada", "Russia",
                "Australia", "Brazil", "China", "Botswana", "Angola",
                "Namibia", "Lesotho", "Zimbabwe", "Sierra Leone", "Tanzania",
                "Belgium", "United Kingdom", "Israel", "Thailand", "United Arab Emirates"
            };

            Colors = new List<string>
            {
                "Yellow", "Green", "Blue", "Brown", "White",
                "Black", "Purple", "Orange", "Pink", "Red",
                "Champagne", "Gray", "Violet", "Cognac", "Fancy Green",
                "Fancy Blue", "Fancy Yellow", "Fancy Brown", "Fancy Purple", "Fancy Orange"
            };

            Cuts = new List<string>
            {
                "Round", "Princess", "Emerald", "Asscher", "Marquise",
                "Oval", "Radiant", "Pear", "Heart", "Cushion",
                "Trillion", "Baguette", "Rose", "Briolette", "Old European",
                "Old Mine", "Tapered Baguette", "Half Moon", "Bullet", "French"
            };
        }
    }
}
