using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.User;

public class ResetPassword : PageModel
{
    [BindProperty]
    [Required]
    public string? Token { get; set; }
    
    [BindProperty]
    [Required]
    public string? Email { get; set; }
    
    [BindProperty]
    [Required]
    public string? Password { get; set; }
    
    [BindProperty]
    [Required]
    [Compare(nameof(Password), ErrorMessage = "Password and Confirm Password do not match")]
    public string? ConfirmPassword { get; set; }
    
    private readonly IUserServices _userServices;

    public ResetPassword(IUserServices userServices)
    {
        _userServices = userServices;
    }
    
    public async Task OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return;
        }
        
        if (Password != ConfirmPassword)
        {
            ModelState.AddModelError("ConfirmPassword", "Password and Confirm Password do not match");
            return;
        }
        
        if (Email == null || Token == null || Password == null || ConfirmPassword == null)
        {
            ModelState.AddModelError("Error", "Invalid request");
            return;
        }
        
        var isSuccess = await _userServices.ResetPassword(Email, Token, ConfirmPassword);
        
        if (!isSuccess)
        {
            ModelState.AddModelError("Error", "Cannot reset password");
        }
    }
}