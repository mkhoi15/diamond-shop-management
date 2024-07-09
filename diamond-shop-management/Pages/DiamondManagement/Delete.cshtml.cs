using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.DiamondDto;
using DTO.Media;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using Services.Abstraction;
using System.Drawing.Printing;
using System.Text.Json;

namespace diamond_shop_management.Pages.DiamondManagement
{
    [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Manager)}")]
    public class DeleteModel : PageModel
    {
        private readonly IDiamondServices _diamondService;
        private readonly IPaperworkServices _paperworkService;
        private readonly IMediaServices _mediaServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeleteModel(IDiamondServices diamondService, IMapper mapper, IMediaServices mediaServices, IUnitOfWork unitOfWork, IPaperworkServices paperworkService)
        {
            _diamondService = diamondService;
            _mapper = mapper;
            _mediaServices = mediaServices;
            _unitOfWork = unitOfWork;
            _paperworkService = paperworkService;
        }

        [BindProperty(SupportsGet = true)]
        public DiamondResponse Diamond { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid DiamondId { get; set; }

        public string? Message { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid diamondId, int? pageNumber, CancellationToken cancellationToken)
        {
            Diamond = await _diamondService.GetByIdAsync(diamondId, default);

            if (Diamond == null)
            {
                Message = "Diamond is not found";
                ModelState.AddModelError(string.Empty, Message);
                return Page();
            }

            TempData["Media"] = JsonSerializer.Serialize(Diamond.Media);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            try
            {

                var jsonMedia = TempData["Media"] as string;
                var media = JsonSerializer.Deserialize<MediaResponse>(jsonMedia);

                if (Diamond.IsDeleted == true)
                {
                    Message = "Diamond is already deleted";
                    ModelState.AddModelError(string.Empty, Message);
                    Diamond.Media = media;
                    TempData["Media"] = jsonMedia;
                    return Page();
                }
                
                if (media != null)
                {
                    media.IsDeleted = true;
                    _mediaServices.Update(media);
                }


                Diamond.IsDeleted = true;

                var paperworks = await _paperworkService.GetAllAsync(paper => paper.DiamondId == Diamond.Id, default);
                foreach (var paperwork in paperworks)
                {
                    paperwork.IsDeleted = true;

                    if (paperwork.Media != null)
                    {
                        paperwork.Media.IsDeleted = true;
                        _mediaServices.Update(paperwork.Media);
                    }
                    _paperworkService.Update(paperwork);
                }

                _diamondService.UpdateDiamond(Diamond);
                await _unitOfWork.SaveChangeAsync(cancellationToken);

                return RedirectToPage("/DiamondManagement/ViewDiamond");
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
