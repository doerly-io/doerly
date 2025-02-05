using System.ComponentModel.DataAnnotations.Schema;
using Doerly.DataAccess.Models;
using Doerly.Module.Profile.DataAccess.Constants;
using Doerly.Module.Profile.DataAccess.Dicts;

namespace Doerly.Module.Profile.DataAccess.Models;

[Table(DbConstants.Tables.Profile, Schema = DbConstants.ProfileSchema)]
public class Profile : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    // public int? AddressId { get; set; }
    // public virtual Address? Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Sex? Sex { get; set; }
    public string? Bio { get; set; }
    public int UserId { get; set; }
}   