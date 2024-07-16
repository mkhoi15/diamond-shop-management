using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.DiamondDto;
using DTO.Media;
using DTO.PaperworkDto;
using DTO.PromotionDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Services.Abstraction;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace diamond_shop_management.Pages.DiamondManagement
{
    [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Manager)}")]
    public class UpdateModel : PageModel
    {
        private readonly IDiamondServices _diamondService;
        private readonly IPaperworkServices _paperworkService;
        private readonly IPromotionServices _promotionServices;
        private readonly IMediaServices _mediaServices;
        private readonly IWebHostEnvironment _environment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateModel(IDiamondServices diamondService, IPaperworkServices paperworkService, IMapper mapper, IUnitOfWork unitOfWork, IWebHostEnvironment environment, IMediaServices mediaServices, IPromotionServices promotionServices)
        {
            _diamondService = diamondService;
            _paperworkService = paperworkService;
            PageSize = 4;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _environment = environment;
            _mediaServices = mediaServices;
            _promotionServices = promotionServices;
        }

        [BindProperty(SupportsGet = true)]
        public DiamondResponse Diamond { get; set; }

        [BindProperty]
        public IFormFile? ImageFile { get; set; }
        public List<string> Countries { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Cuts { get; set; }

        public List<PaperworkResponse> Paperworks { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid diamondId, int? pageNumber, CancellationToken cancellationToken)
        {
            try
            {
                await InitializeDiamondInfoAsync();
                Diamond = await _diamondService.GetByIdAsync(diamondId, default);


                if (Diamond == null)
                {
                    Message = "Diamond is not found";
                    ModelState.AddModelError(string.Empty, Message);
                    return Page();
                }

                TempData["Media"] = JsonSerializer.Serialize(Diamond.Media);

                PageNumber = pageNumber ?? 1;

                var pagedResult = await _paperworkService.GetAllAsync(
                    paper => paper.DiamondId == diamondId && paper.IsDeleted != true, PageNumber, PageSize, cancellationToken);

                Paperworks = _mapper.Map<List<PaperworkResponse>>(pagedResult.Items);
                TotalItems = pagedResult.TotalItems;

                return Page();
            }
            catch (Exception ex)
            {
                Message = "Get Diamond info failed!\n" + ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid diamondId, CancellationToken cancellationToken)
        {
            try
            {

                MediaResponse? media = await SaveMedia(ImageFile);

                Diamond.Id = diamondId;

                var jsonMedia = TempData["Media"] as string;

                var updatedMedia = JsonSerializer.Deserialize<MediaResponse>(jsonMedia);
                if (media != null)
                {
                    if (updatedMedia != null && updatedMedia.Url != null)
                    {
                        DeleteFile(updatedMedia.Url);
                        updatedMedia.Url = media.Url;
                        _mediaServices.Update(updatedMedia);
                    }
                    else
                    {
                        var addMedia = _mediaServices.Add(media);
                        Diamond.MediaId = addMedia.Id;
                    }
                }
                _diamondService.UpdateDiamond(Diamond);
                await _unitOfWork.SaveChangeAsync(cancellationToken);

                /*Message = "Diamond is updated successfully";
                ModelState.AddModelError(string.Empty, Message);*/

                await InitializeDiamondInfoAsync();
                return RedirectToPage("/DiamondManagement/ViewDiamond");
            }
            catch (Exception ex)
            {
                Message = "Update failed!\n" + ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);

                await InitializeDiamondInfoAsync();
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

        public async Task InitializeDiamondInfoAsync()
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
            IEnumerable<PromotionResponse> promotions = await _promotionServices.GetAllAsync(default);
            ViewData["Promotions"] = new SelectList(promotions, "Id", "Name");
        }

        public void DeleteFile(string mediaUrl)
        {
            string relativePath = mediaUrl;
            string physicalPath = Path.Combine(_environment.WebRootPath, relativePath);

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }
        }
    }
}
