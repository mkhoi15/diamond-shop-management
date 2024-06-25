using BusinessObject.Enum;
using BusinessObject.Models;
using DTO;
using DTO.UserDto;
using Microsoft.AspNetCore.Http;

namespace Services.Abstraction;

public interface IUserServices
{
    Task<(List<UserResponse>, int totalPage)> GetAllUserAsync(int page = 1, int limit = 10);
    Task<bool> IsSignedInAsync(UserResponse user);
    Task<UserResponse> Login(string username, string password);
    Task SignOutAsync();
    Task<UserResponse> RegisterAsync(string username, string password, string fullname, string email, string? phone, Roles roles);
    Task<UserResponse> GetUserByIdAsync(string id);
    Task<UserResponse> UpdateUserAsync(string id, string fullname, string email, string? phone, Roles roles);
    Task<bool> ChangePassword(string username, string oldPassword, string newPassword);
    Task<bool> ResetPassword(string email, string token, string newPassword);
    Task ForgotPassword(string username);
}