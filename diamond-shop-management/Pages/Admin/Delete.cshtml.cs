using BusinessObject.Enum;
using DTO.UserDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Admin;

public class Delete : PageModel
{
    private readonly IUserServices _userServices;
    
    [BindProperty]
    public UserResponse UserResponse { get; set; }
    
    public string? ErrorMessage { get; set; }

    public Delete(IUserServices userServices)
    {
        _userServices = userServices;
    }

    public async Task<IActionResult> OnGet(string? id)
    {
        if (id is null)
        {
            return NotFound();
        }
        var user = await _userServices.GetUserByIdAsync(id);
        UserResponse = new UserResponse
        {
            Id = user.Id ?? "",
            UserName = user.UserName ?? "",
            FullName = user.FullName ?? "",
            Email = user.Email ?? "",
            PhoneNumber = user.PhoneNumber ?? "",
            Role = user.Role ?? ""
        };
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (UserResponse.Id is null)
            {
                return NotFound();
            }

            var user = await _userServices.GetUserByIdAsync(UserResponse.Id);
            UserResponse = user;
            await _userServices.DeleteUserAsync(UserResponse.Id!);

            return RedirectToPage("./Index");
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            return Page();
        }
    }
}