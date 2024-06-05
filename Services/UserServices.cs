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

        await _signInManager.PasswordSignInAsync(user, password, 
            isPersistent: false, 
            lockoutOnFailure: false);
        
        return _mapper.Map<UserResponse>(user);
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<UserResponse> RegisterAsync(string username, string password, string fullname, string? phone)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is not null)
        {
            throw new AuthenticationException("User is exist!!");
        }

        var newUser = new User()
        {
            UserName = username,
            PhoneNumber = phone
        };

        var result = await _userManager.CreateAsync(newUser, password);
        if (!result.Succeeded) throw new Exception("Error when creating user!!");
        
        var response = _mapper.Map<UserResponse>(newUser);
        return response;

    }
}