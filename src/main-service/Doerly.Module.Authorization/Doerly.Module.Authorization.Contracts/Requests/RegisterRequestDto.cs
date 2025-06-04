using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Authorization.Contracts.Requests;

public class RegisterRequestDto
{
    [Required(ErrorMessage = "InvalidEmailFormatInput")]
    [EmailAddress(ErrorMessage = "InvalidEmailFormatInput")]
    public string Email { get; set; }

    [Required(ErrorMessage = "InvalidPasswordInput")]
    public string Password { get; set; }

    [MaxLength(50, ErrorMessage = "FirstNameTooLong")]
    [MinLength(1, ErrorMessage = "FirstNameTooShort")]
    public string FirstName { get; set; }

    [MaxLength(50, ErrorMessage = "LastNameTooLong")]
    [MinLength(1, ErrorMessage = "LastNameTooShort")]
    public string LastName { get; set; }
}
