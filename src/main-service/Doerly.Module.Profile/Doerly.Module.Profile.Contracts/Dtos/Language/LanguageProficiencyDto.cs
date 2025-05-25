using Doerly.Module.Profile.Enums;

namespace Doerly.Module.Profile.Contracts.Dtos;

public class LanguageProficiencyDto
{
    public int Id { get; set; }
    public required LanguageDto Language { get; set; }
    public required ELanguageProficiencyLevel Level { get; set; }
}