using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.User;

public class ForgotPassword : PageModel
{
    [Required]
    [BindProperty]
    public string Email { get; set; }
    private readonly IUserServices _userServices;

    public ForgotPassword(IUserServices userServices)
    {
        _userServices = userServices;
    }

    public void OnGet()
    {
    }
    
    public async Task OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return;
        }
        await _userServices.ForgotPassword(Email);
    }
}