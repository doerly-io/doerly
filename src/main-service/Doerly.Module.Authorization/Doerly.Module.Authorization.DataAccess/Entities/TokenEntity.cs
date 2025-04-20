using System.ComponentModel.DataAnnotations.Schema;
using Doerly.Module.Authorization.DataAccess.Constants;
using Doerly.Module.Authorization.Enums;

namespace Doerly.Module.Authorization.DataAccess.Entities;

[Table(DbConstants.Tables.Token, Schema = DbConstants.AuthSchema)]
public class TokenEntity
{
    public Guid Guid { get; set; }

    public string Value { get; set; }

    public DateTime DateCreated { get; set; }

    public int UserId { get; set; }

    public virtual UserEntity User { get; set; }

    public ETokenKind TokenKind { get; set; }
}
