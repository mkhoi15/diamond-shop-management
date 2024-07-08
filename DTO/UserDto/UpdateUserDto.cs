using System.ComponentModel.DataAnnotations;

namespace DTO.UserDto;

public class UpdateUserDto
{
    [Required]
    public string Id { get; set; } = null!;
    
    [Required]
    public string UserName { get; set; } = null!;
    
    [Required]
    public string FullName { get; set; } = null!;
    
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = null!;
    
    [Phone(ErrorMessage = "Invalid phone number.")]
    [Length(minimumLength: 10, maximumLength: 11, ErrorMessage = "Phone number must be between 10 and 11 digits.")]
    public string Phone { get; set; } = null!;
}