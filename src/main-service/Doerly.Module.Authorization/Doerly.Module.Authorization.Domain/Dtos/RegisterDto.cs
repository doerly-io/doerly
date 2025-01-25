using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Authorization.Domain.Dtos;

public class RegisterDto
{
    [Required]
    [DataType(DataType.EmailAddress, ErrorMessage = "InvalidEmailFormatInput")]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password, ErrorMessage = "InvalidPasswordInput")]
    public string Password { get; set; }
    
}
