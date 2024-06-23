using BusinessObject.Enum;
using DTO.UserDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.Abstraction;

namespace diamond_shop_management.Pages.Admin;

[Authorize(Roles = nameof(Roles.Admin))]
public class Index : PageModel
{
    public IList<UserResponse> Users { get; set; } = new List<UserResponse>();
    public int TotalPage { get; set; }
    public int CurrentPage { get; set; } = 1;
    
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPage;
    
    private readonly IUserServices _userServices;

    public Index(IUserServices userServices)
    {
        _userServices = userServices;
    }

    public async Task OnGetAsync(int currentPage = 1)
    {
        CurrentPage = currentPage;
        (Users, TotalPage) = await _userServices.GetAllUserAsync(currentPage);
    }
}