using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Authorization.Domain.Dtos;

public class ResetPasswordDto
{
    [Required]
    [DataType(DataType.EmailAddress, ErrorMessage = "InvalidEmailFormatInput")]
    public string Email { get; set; }
    
    [Required]
    public Guid Token { get; set; }
    
    [Required]
    public string Password { get; set; }
}
