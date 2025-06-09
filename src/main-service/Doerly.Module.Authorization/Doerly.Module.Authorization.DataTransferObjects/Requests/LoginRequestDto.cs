using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Authorization.DataTransferObjects.Requests;

public class LoginRequestDto
{
    [Required(ErrorMessage = "InvalidEmailFormatInput")]
    [EmailAddress(ErrorMessage = "InvalidEmailFormatInput")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "FieldIsRequired")]
    public string Password { get; set; }
}
