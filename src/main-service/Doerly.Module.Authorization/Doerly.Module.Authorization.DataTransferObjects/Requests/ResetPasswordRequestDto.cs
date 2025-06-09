using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Authorization.DataTransferObjects.Requests;

public class ResetPasswordRequestDto
{
    
    [Required(ErrorMessage = "InvalidEmailFormatInput")]
    [EmailAddress(ErrorMessage = "InvalidEmailFormatInput")]
    public string Email { get; set; }
    
    [Required]
    public string Token { get; set; }
    
    [Required(ErrorMessage = "InvalidPasswordInput")]
    public string Password { get; set; }
}
