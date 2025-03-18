using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Authorization.Contracts.Dtos;

public class LoginRequestDto
{
    [Required(ErrorMessage = "InvalidEmailFormatInput")]
    [EmailAddress(ErrorMessage = "InvalidEmailFormatInput")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "PasswordRequired")]
    public string Password { get; set; }
}
