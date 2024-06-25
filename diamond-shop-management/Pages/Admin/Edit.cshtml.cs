using System.ComponentModel.DataAnnotations;
using BusinessObject.Enum;
using DTO.UserDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Admin;

public class Edit : PageModel
{
    private readonly IUserServices _userServices;
    
    [BindProperty]
    public UpdateUserDto UpdateUserDto { get; set; }
    
    [BindProperty]
    [Required]
    public Roles Role { get; set; }
    
    public string ReturnUrl { get; set; }

    public Edit(IUserServices userServices)
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
        UpdateUserDto = new UpdateUserDto
        {
            Id = user.Id ?? "",
            UserName = user.UserName ?? "",
            FullName = user.FullName ?? "",
            Email = user.Email ?? "",
            Phone = user.PhoneNumber ?? ""
        };
        Role = Enum.Parse<Roles>(user.Role!);
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        try
        {
            var user = await _userServices.UpdateUserAsync(UpdateUserDto.Id, 
                UpdateUserDto.FullName,
                UpdateUserDto.Email,
                UpdateUserDto.Phone,
                Role);
            
            
            return RedirectToPage("./Index");;
        }
        catch (Exception e)
        {
            ModelState.AddModelError("", e.Message);
            return Page();
        }
        
    }
}