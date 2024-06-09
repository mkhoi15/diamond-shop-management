using BusinessObject.Enum;
using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.User;

public class Logout : PageModel
{
    private readonly IUserServices _userServices;

    public Logout(IUserServices userServices)
    {
        _userServices = userServices;
    }

    public void OnGet()
    {
    }
    
    public async Task<IActionResult> OnPost(string? returnUrl = null)
    {
        await _userServices.SignOutAsync();
        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            // This needs to be a redirect so that the browser performs a new
            // request and the identity for the user gets updated.
            return RedirectToPage();
        }
    }
}