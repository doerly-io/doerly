using System.ComponentModel.DataAnnotations.Schema;
using Doerly.DataAccess.Models;
using Doerly.Module.Profile.DataAccess.Constants;

namespace Doerly.Module.Profile.DataAccess.Models;

[Table(DbConstants.Tables.Language, Schema = DbConstants.ProfileSchema)]
public class Language : BaseEntity
{
    public string Name { get; set; }
    public string Code { get; set; }
    
    public virtual ICollection<LanguageProficiency> LanguageProficiencies { get; set; } = new List<LanguageProficiency>();
}