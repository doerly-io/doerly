using System.ComponentModel.DataAnnotations.Schema;
using Doerly.DataAccess.Models;
using Doerly.Module.Authorization.DataAccess.Constants;

namespace Doerly.Module.Authorization.DataAccess.Models;

[Table(DbConstants.Tables.Role, Schema = DbConstants.AuthSchema)]
public class Role : BaseEntity
{
    public string Name { get; set; }
    public virtual ICollection<User> Users { get; set; }
}