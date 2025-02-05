using System.ComponentModel.DataAnnotations.Schema;
using Doerly.Module.Authorization.DataAccess.Constants;

namespace Doerly.Module.Authorization.DataAccess.Models;

[Table(DbConstants.Tables.ResetToken, Schema = DbConstants.AuthSchema)]
public class ResetToken
{
    public Guid Guid { get; set; }

    public DateTime DateCreated { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; }
}
