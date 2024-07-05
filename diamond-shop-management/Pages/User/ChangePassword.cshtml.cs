using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.User;

public class ChangePassword : PageModel
{
    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }
    
    [Required(ErrorMessage = "Old password is required")]
    [DataType(DataType.Password)]
    public string? OldPassword { get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "New password is required")]
    public string? NewPassword { get; set; }
    
    [Required(ErrorMessage = "Confirm password is required")]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Password and confirm password do not match")]
    public string? ConfirmPassword { get; set; }
    
    private readonly IUserServices _userServices;

    public ChangePassword(IUserServices userServices)
    {
        _userServices = userServices;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        // Change password logic
        await _userServices.ChangePassword(Username!, OldPassword!, NewPassword!);
        
        return RedirectToPage("/Index");
    }
}