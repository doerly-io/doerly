using System.ComponentModel.DataAnnotations;
using Doerly.Module.Profile.Enums;

namespace Doerly.Module.Profile.Contracts.Dtos;

public class LanguageProficiencySaveDto
{
    [Required]
    public int LanguageId { get; set; }
    
    [Required]
    [EnumDataType(typeof(ELanguageProficiencyLevel), ErrorMessage = "InvalidLanguageProficiencyLevelInput")]
    public ELanguageProficiencyLevel Level { get; set; }
}
