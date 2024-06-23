using System.Security.Authentication;
using System.Web;
using AutoMapper;
using BusinessObject.Enum;
using BusinessObject.Models;
using DTO.UserDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Abstraction;

namespace Services;

public class UserServices : IUserServices
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IMapper _mapper;
    private readonly IEmailServices _emailServices;
    private readonly RoleManager<Role> _roleManager;

    public UserServices(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper,
        IEmailServices emailServices, RoleManager<Role> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _emailServices = emailServices;
        _roleManager = roleManager;
    }

    public async Task<(List<UserResponse>, int totalPage)> GetAllUserAsync(int page = 1, int limit = 10)
    {
        var response = new List<UserResponse>();

        var users = await _userManager.Users
            .AsNoTracking()
            .Take(limit)
            .Skip((page - 1) * limit)
            .ToListAsync();
        var count = await _userManager.Users.CountAsync();

        var totalPage = (int)Math.Ceiling(decimal.Divide(count, limit));
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Role = roles.FirstOrDefault();
            response.Add(userResponse);
        }

        return (response, totalPage);
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

        var result = await _signInManager.PasswordSignInAsync(user, password,
            isPersistent: false,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new AuthenticationException("User name or password wrong!!");
        }

        return _mapper.Map<UserResponse>(user);
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<UserResponse> RegisterAsync(string username, string password, string fullname,
        string email, string? phone, Roles roles)
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
            PhoneNumber = phone,
            Email = email,
            FullName = fullname
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

    public async Task<bool> ResetPassword(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new AuthenticationException("User name does not exist!!");
        }

        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        return result.Succeeded;
    }

    public async Task ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new AuthenticationException("Email does not exist!!");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        await _emailServices.SendForgotPasswordMail(user.Email!, token);
    }
}