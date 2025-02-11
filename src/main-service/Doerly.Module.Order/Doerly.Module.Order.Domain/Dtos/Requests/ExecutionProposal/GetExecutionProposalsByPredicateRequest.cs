using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Order.Domain.Dtos.Requests.ExecutionProposal;
public class GetExecutionProposalsByPredicateRequest
{
    public int? ReceiverId { get; set; }

    public int? SenderId { get; set; }
}
