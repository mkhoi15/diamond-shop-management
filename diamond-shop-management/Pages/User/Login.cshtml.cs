using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.User;

[Authorize]
public class Login : PageModel
{
    private readonly IUserServices _userServices;
    public string Username { get; set; }
    public string Password { get; set; }
    
    public Login(IUserServices userServices)
    {
        _userServices = userServices;
    }

    public void OnGet()
    {
    }

    public async Task OnPost()
    {
        await _userServices.Login(Username, Password);
    }
}