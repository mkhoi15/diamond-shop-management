using System.Security.Authentication;
using AutoMapper;
using BusinessObject.Enum;
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
    private readonly IEmailServices _emailServices;
    private readonly RoleManager<Role> _roleManager;

    public UserServices(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IEmailServices emailServices, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _emailServices = emailServices;
        _roleManager = roleManager;
    }

    public Task<bool> IsSignedInAsync(UserResponse user)
    {
        //_signInManager.IsSignedIn();
        return Task.FromResult(true);
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

    public async Task<UserResponse> RegisterAsync(string username, string password, string fullname, 
        string? phone, Roles roles)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is not null)
        {
            throw new AuthenticationException("User is exist!!");
        }

        if (!await _roleManager.RoleExistsAsync(roles.ToString()))
        {
            var role = new Role()
            {
                Name = roles.ToString()
            };
            var roleResult = await _roleManager.CreateAsync(role);
            if (!roleResult.Succeeded) throw new Exception("Error when creating role!!");
        }
        
        var newUser = new User()
        {
            UserName = username,
            PhoneNumber = phone
        };

        var result = await _userManager.CreateAsync(newUser, password);
        if (!result.Succeeded) throw new Exception("Error when creating user!!");
        await _userManager.AddToRoleAsync(newUser, roles.ToString());
        
        var response = _mapper.Map<UserResponse>(newUser);
        return response;
    }

    public async Task<bool> ChangePassword(string username, string oldPassword, string newPassword)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            throw new AuthenticationException("User name or password wrong!!");
        }
        
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        return result.Succeeded;
    }

    public async Task<bool> ResetPassword(string username, string token, string newPassword)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null)
        {
            throw new AuthenticationException("User name does not exist!!");
        }
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        return result.Succeeded;
    }
    
       public async Task ForgotPassword(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
            {
                throw new AuthenticationException("User name or password wrong!!");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailServices.SendForgotPasswordMail(user.Email!, token);
        }
}