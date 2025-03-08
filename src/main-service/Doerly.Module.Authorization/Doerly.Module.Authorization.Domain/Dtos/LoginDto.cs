using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Authorization.Domain.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "InvalidEmailFormatInput")]
    [EmailAddress(ErrorMessage = "InvalidEmailFormatInput")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "PasswordRequired")]
    public string Password { get; set; }
}
