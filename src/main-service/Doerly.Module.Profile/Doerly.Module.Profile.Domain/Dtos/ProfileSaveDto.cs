using System.ComponentModel.DataAnnotations;
using Doerly.Module.Profile.Enums;

namespace Doerly.Module.Profile.Domain.Dtos;

public class ProfileSaveDto
{
    [Required]
    public int UserId { get; set; }
    
    [DataType(DataType.Text, ErrorMessage = "InvalidFirstNameInput")]
    [MaxLength(50, ErrorMessage = "FirstNameTooLong")]
    [MinLength(1, ErrorMessage = "FirstNameTooShort")]
    public string FirstName { get; set; }
    
    [DataType(DataType.Text, ErrorMessage = "InvalidLastNameInput")]
    [MaxLength(50, ErrorMessage = "LastNameTooLong")]
    [MinLength(1, ErrorMessage = "LastNameTooShort")]
    public string LastName { get; set; }
    
    [DataType(DataType.Date, ErrorMessage = "InvalidDateOfBirthInput")]
    public DateOnly? DateOfBirth { get; set; }
    
    [EnumDataType(typeof(ESex), ErrorMessage = "InvalidSexInput")]
    public ESex Sex { get; set; }
    
    [DataType(DataType.MultilineText, ErrorMessage = "InvalidBioInput")]
    [MaxLength(500, ErrorMessage = "BioTooLong")]
    public string? Bio { get; set; }
}