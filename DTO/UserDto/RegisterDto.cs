using System.ComponentModel.DataAnnotations;

namespace DTO.UserDto;

public class RegisterDto
{
    [Required] public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    
    public string? FullName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    
    [Required]
    [Phone]
    public string Phone { get; set; } = null!;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Password does not match")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;
}