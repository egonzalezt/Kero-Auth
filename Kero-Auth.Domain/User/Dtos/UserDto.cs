using System.ComponentModel.DataAnnotations;

namespace Kero_Auth.Domain.User.Dtos;

public class UserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(6, ErrorMessage = "The Password must be at least 6 characters long.")]
    public string Password { get; set; }
}
