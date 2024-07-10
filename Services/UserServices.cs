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

        var query = _userManager.Users
            .AsNoTracking()
            .Where(u => u.IsDeleted == false && u.UserName != "admin");

        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();
        
        var count = await query
            .CountAsync();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Role = roles.FirstOrDefault();
            response.Add(userResponse);
        }

        var totalPage = (int)Math.Ceiling(decimal.Divide(count, limit));
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
        var emailExist = await _userManager.FindByEmailAsync(email);
        if (emailExist is not null)
        {
            throw new AuthenticationException("Email is exist!!");
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
            FullName = fullname,
            CreatedAt = DateTime.Now
        };

        var result = await _userManager.CreateAsync(newUser, password);
        if (!result.Succeeded) throw new Exception("Error when creating user!!");
        await _userManager.AddToRoleAsync(newUser, roles.ToString());

        var response = _mapper.Map<UserResponse>(newUser);
        return response;
    }

    public async Task<UserResponse> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            throw new AuthenticationException("User does not exist!!");
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        
        var userResponse = _mapper.Map<UserResponse>(user);
        userResponse.Role = roles.FirstOrDefault();
        
        return userResponse;
    }

    public async Task<UserResponse> UpdateUserAsync(string id, string fullname, string email, string? phone, Roles roles)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            throw new AuthenticationException("User does not exist!!");
        }
        
        user.FullName = fullname;
        user.Email = email;
        user.PhoneNumber = phone;
        
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) throw new Exception("Error when updating user!!");
        
        var userRoles = await _userManager.GetRolesAsync(user);
        
        if (userRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, userRoles);
        }
        
        var isSuccess = await _userManager.AddToRoleAsync(user, roles.ToString());
        if (!isSuccess.Succeeded) throw new Exception("Error when updating user role!!");
        
        var response = _mapper.Map<UserResponse>(user);
        response.Role = roles.ToString();
        
        return response;
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            throw new AuthenticationException("User does not exist!!");
        }
        
        user.IsDeleted = true;
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
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

    public async Task<List<UserResponse>> GetDeliveryMenAsync(CancellationToken cancellationToken)
    {
        var deliveryMenRole = Roles.Delivery.ToString();

        // First, get all users
        var users = await _userManager.Users
            .Where(u => u.IsDeleted == false)
            .ToListAsync(cancellationToken);

        // Then, filter users based on their roles
        var deliveryMen = new List<User>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains(deliveryMenRole))
            {
                deliveryMen.Add(user);
            }
        }

        var response = deliveryMen.Select(user => {
            var userResponse = _mapper.Map<UserResponse>(user);
            userResponse.Role = deliveryMenRole;
            return userResponse;
        }).ToList();

        return response;
    }
}