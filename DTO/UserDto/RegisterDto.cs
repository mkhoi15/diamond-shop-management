using System.ComponentModel.DataAnnotations;

namespace DTO.UserDto;

public class RegisterDto
{
    [Required] public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required] public string Password { get; set; } = null!;
    
    [Required]
    [Phone]
    public string Phone { get; set; } = null!;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Password does not match")]
    public string ConfirmPassword { get; set; } = null!;
}