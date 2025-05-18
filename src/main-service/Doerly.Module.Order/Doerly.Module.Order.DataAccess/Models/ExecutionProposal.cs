using Doerly.DataAccess.Models;
using Doerly.Module.Order.DataAccess.Constants;
using Doerly.Module.Order.DataAccess.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doerly.Module.Order.DataAccess.Models;

public class ExecutionProposal : BaseEntity
{
    public int OrderId { get; set; }

    public string? Comment { get; set; }
    
    public int SenderId { get; set; }

    public int ReceiverId { get; set; }

    public EExecutionProposalStatus Status { get; set; }

    public virtual Order Order { get; set; }
}
