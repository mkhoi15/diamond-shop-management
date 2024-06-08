using BusinessObject.Models;
using DTO;
using DTO.UserDto;
using Microsoft.AspNetCore.Http;

namespace Services.Abstraction;

public interface IUserServices
{
    Task<UserResponse> Login(string username, string password);
    Task SignOutAsync();
    Task<UserResponse> RegisterAsync(string username, string password, string fullname, string? phone);
    Task<bool> ChangePassword(string username, string oldPassword, string newPassword);
}