using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Profile.Contracts.Dtos;

public class LanguageSaveDto
{
    [Required]
    [MaxLength(100, ErrorMessage = "LanguageNameTooLong")]
    [MinLength(1, ErrorMessage = "LanguageNameTooShort")]
    public required string Name { get; set; }
    
    [Required]
    [MaxLength(10, ErrorMessage = "LanguageCodeTooLong")]
    [MinLength(2, ErrorMessage = "LanguageCodeTooShort")]
    public required string Code { get; set; }
}