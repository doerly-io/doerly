using System.ComponentModel.DataAnnotations.Schema;
using Doerly.DataAccess.Models;
using Doerly.Module.Authorization.DataAccess.Constants;

namespace Doerly.Module.Authorization.DataAccess.Entities;

[Table(DbConstants.Tables.User, Schema = DbConstants.AuthSchema)]
public class UserEntity : BaseEntity
{
    public required string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }

    public bool IsEmailVerified { get; set; }
    public int? RoleId { get; set; }
    public virtual RoleEntity? Role { get; set; }

    public virtual ICollection<TokenEntity> Tokens { get; set; }
}
