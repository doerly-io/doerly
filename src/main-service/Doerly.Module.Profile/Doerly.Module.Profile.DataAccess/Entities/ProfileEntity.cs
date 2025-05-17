using System.ComponentModel.DataAnnotations.Schema;
using Doerly.DataAccess.Models;
using Doerly.Module.Profile.DataAccess.Constants;
using Doerly.Module.Profile.Enums;

namespace Doerly.Module.Profile.DataAccess.Models;

[Table(DbConstants.Tables.Profile, Schema = DbConstants.ProfileSchema)]
public class ProfileEntity : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    // public int? AddressId { get; set; }
    // public virtual Address? Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public ESex Sex { get; set; }
    public string? Bio { get; set; }
    public int UserId { get; set; }
    public string? ImagePath { get; set; }
    public string? CvPath { get; set; }
}   
