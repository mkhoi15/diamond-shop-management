using System.ComponentModel.DataAnnotations;
using BusinessObject.Enum;
using DTO.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Admin;

[Authorize(Roles = nameof(Roles.Admin))]
public class Create : PageModel
{
    private readonly IUserServices _userServices;

    [BindProperty] public RegisterDto RegisterDto { get; set; }
    
    [BindProperty]
    [Required]
    public Roles Role { get; set; } = Roles.Manager;

    public string ReturnUrl { get; set; }

    public Create(IUserServices userServices)
    {
        _userServices = userServices;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        try
        {
            var user = await _userServices.RegisterAsync(RegisterDto.Username,
                RegisterDto.Password,
                RegisterDto.Username,
                RegisterDto.Email,
                RegisterDto.Phone,
                Role);
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return Page();
        }
        
        return RedirectToPage("./Index");
    }
}