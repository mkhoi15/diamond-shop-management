using DTO.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.User;

public class Login : PageModel
{
    private readonly IUserServices _userServices;
    
    [BindProperty]
    public LoginDto LoginDto { get; set; }
    
    public Login(IUserServices userServices)
    {
        _userServices = userServices;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            await _userServices.Login(LoginDto.Username, LoginDto.Password);
            
            return RedirectToPage("/Index");
        }
        catch (Exception e)
        {
            ModelState.AddModelError("Error", e.Message);
            return Page();
        }
        
    }
}