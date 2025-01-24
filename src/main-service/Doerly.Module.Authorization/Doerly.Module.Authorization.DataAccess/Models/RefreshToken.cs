using System.ComponentModel.DataAnnotations.Schema;
using Doerly.DataAccess.Models;
using Doerly.Module.Authorization.DataAccess.Constants;

namespace Doerly.Module.Authorization.DataAccess.Models;

[Table(DbConstants.Tables.RefreshToken, Schema = DbConstants.AuthSchema)]
public class RefreshToken : BaseEntity
{
    public Guid Guid { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; }
}