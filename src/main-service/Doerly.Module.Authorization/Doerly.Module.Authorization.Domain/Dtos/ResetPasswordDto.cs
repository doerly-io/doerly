using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Authorization.Domain.Dtos;

public class ResetPasswordDto
{
    [Required]
    public string Token { get; set; }
    
    [Required(ErrorMessage = "InvalidPasswordInput")]
    public string Password { get; set; }
}
