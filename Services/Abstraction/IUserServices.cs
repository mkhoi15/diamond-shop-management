using BusinessObject.Models;
using DTO;
using DTO.UserDto;

namespace Services.Abstraction;

public interface IUserServices
{
    Task<UserResponse> Login(string username, string password);
}