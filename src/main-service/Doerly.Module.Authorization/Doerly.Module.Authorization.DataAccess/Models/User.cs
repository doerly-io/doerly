using System.ComponentModel.DataAnnotations.Schema;
using Doerly.DataAccess.Models;
using Doerly.Module.Authorization.DataAccess.Constants;

namespace Doerly.Module.Authorization.DataAccess.Models;

[Table(DbConstants.Tables.User, Schema = DbConstants.AuthSchema)]
public class User : BaseEntity
{
    public required string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public int? RoleId { get; set; }
    public virtual Role? Role { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
}