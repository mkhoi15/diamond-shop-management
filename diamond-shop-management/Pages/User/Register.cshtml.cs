using BusinessObject.Enum;
using DTO.UserDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.User;

public class Register : PageModel
{
    private readonly IUserServices _userServices;
    
    [BindProperty]
    public RegisterDto RegisterDto { get; set; }
    
    public string ReturnUrl { get; set; }

    public Register(IUserServices userServices)
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
                Roles.User);

            var userLogin = await _userServices.Login(user.UserName!, RegisterDto.Password);
        }
        catch (Exception e)
        {
            ModelState.AddModelError("Error", e.Message);
            return Page();
        }
        return Redirect("/Index");
    }
}