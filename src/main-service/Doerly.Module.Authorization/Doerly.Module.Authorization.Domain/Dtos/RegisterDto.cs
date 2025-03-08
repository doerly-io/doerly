using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Authorization.Domain.Dtos;

public class RegisterDto
{
    [Required(ErrorMessage = "InvalidEmailFormatInput")]
    [EmailAddress(ErrorMessage = "InvalidEmailFormatInput")]
    public string Email { get; set; }

    [Required(ErrorMessage = "InvalidPasswordInput")]
    public string Password { get; set; }
}
