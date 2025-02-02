using Doerly.DataAccess.Models;
using Doerly.Module.Order.DataAccess.Constants;
using Doerly.Module.Order.DataAccess.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doerly.Module.Order.DataAccess.Models;

[Table(DbConstants.Tables.ExecutionProposal, Schema = DbConstants.OrderSchema)]
public class ExecutionProposal : BaseEntity
{
    public int OrderId { get; set; }

    public virtual Order Order { get; set; }

    public string? Comment { get; set; }
    
    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public ExecutionProposalStatus Status { get; set; }
}
