using Doerly.DataAccess.Models;
using Doerly.Module.Profile.Enums;

namespace Doerly.Module.Profile.DataAccess.Entities;

public class LanguageProficiency : BaseEntity
{
    public int LanguageId { get; set; }
    public virtual Language Language { get; set; }
    
    public int ProfileId { get; set; }
    public virtual Profile Profile { get; set; }
    
    public ELanguageProficiencyLevel Level { get; set; }
}
