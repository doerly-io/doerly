using System.ComponentModel.DataAnnotations;
using Doerly.Module.Profile.DataAccess.Dicts;

namespace Doerly.Module.Profile.Domain.Dtos;

public class ProfileSaveDto
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    [DataType(DataType.Text, ErrorMessage = "InvalidFirstNameInput")]
    public string FirstName { get; set; }
    
    [Required]
    [DataType(DataType.Text, ErrorMessage = "InvalidLastNameInput")]
    public string LastName { get; set; }
    
    [DataType(DataType.Text, ErrorMessage = "InvalidPatronymicInput")]
    public string? Patronymic { get; set; }
    
    [DataType(DataType.Date, ErrorMessage = "InvalidDateOfBirthInput")]
    public DateOnly? DateOfBirth { get; set; }
    
    [EnumDataType(typeof(Sex), ErrorMessage = "InvalidSexInput")]
    public Sex? Sex { get; set; }
    
    [DataType(DataType.MultilineText, ErrorMessage = "InvalidBioInput")]
    public string? Bio { get; set; }
}