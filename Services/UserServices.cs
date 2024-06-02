using System.Security.Authentication;
using AutoMapper;
using BusinessObject.Models;
using DTO.UserDto;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction;

namespace Services;

public class UserServices : IUserServices
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;

    public UserServices(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<UserResponse> Login(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            throw new AuthenticationException("User name or password wrong!!");
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        
        return _mapper.Map<UserResponse>(user);
    }
}