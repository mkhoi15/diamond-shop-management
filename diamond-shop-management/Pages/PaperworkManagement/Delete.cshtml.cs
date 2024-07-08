using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.Models;
using DataAccessLayer.Abstraction;
using DTO.Media;
using DTO.PaperworkDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;
using System.Text.Json;

namespace diamond_shop_management.Pages.PaperworkManagement
{
    [Authorize(Roles = $"{nameof(Roles.Admin)},{nameof(Roles.Manager)}")]
    public class DeleteModel : PageModel
    {
        private readonly IPaperworkServices _paperworkServices;
        private readonly IMediaServices _mediaServices;
        private readonly IWebHostEnvironment _environment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        [BindProperty(SupportsGet = true)]
        public PaperworkResponse Paperwork { get; set; }

        public string? Message { get; set; }

        public DeleteModel(IPaperworkServices paperworkServices, IMediaServices mediaServices, IWebHostEnvironment environment, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _paperworkServices = paperworkServices;
            _mediaServices = mediaServices;
            _environment = environment;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
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
                var jsonMedia = TempData["Media"] as string;

                var paperworkMedia = JsonSerializer.Deserialize<MediaResponse>(jsonMedia);

                if (Paperwork.IsDeleted == true || Paperwork.Status == "Inactive")
                {
                    Message = "Paperwork is already deleted";
                    ModelState.AddModelError(string.Empty, Message);
                    Paperwork.Media = paperworkMedia;
                    TempData["Media"] = jsonMedia;
                    return Page();
                }


                Paperwork.IsDeleted = true;
                Paperwork.Status = "Inactive";
                _paperworkServices.Update(Paperwork);
                await _unitOfWork.SaveChangeAsync(cancellationToken);

                return RedirectToPage("/DiamondManagement/Update", new { DiamondId = Paperwork.DiamondId });
            }
            catch (Exception ex)
            {
                Message = "Update failed!\n" + ex.Message;
                ModelState.AddModelError(string.Empty, ex.Message);

                return Page();
            }
        }
    }
}
