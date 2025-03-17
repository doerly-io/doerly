using System.ComponentModel.DataAnnotations.Schema;
using Doerly.DataAccess.Models;
using Doerly.Module.Authorization.DataAccess.Constants;

namespace Doerly.Module.Authorization.DataAccess.Entities;

[Table(DbConstants.Tables.Role, Schema = DbConstants.AuthSchema)]
public class RoleEntity : BaseEntity
{
    public string Name { get; set; }
    public virtual ICollection<UserEntity> Users { get; set; }
}
