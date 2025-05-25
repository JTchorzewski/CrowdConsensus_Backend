using System.ComponentModel.DataAnnotations;

namespace Domain.Model.DTO;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Username { get; set; }
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}