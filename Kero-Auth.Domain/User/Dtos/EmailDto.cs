namespace Kero_Auth.Domain.User.Dtos;

using System.ComponentModel.DataAnnotations;

public class EmailDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
