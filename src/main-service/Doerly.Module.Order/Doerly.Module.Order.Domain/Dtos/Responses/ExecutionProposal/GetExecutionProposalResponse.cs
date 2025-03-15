using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Module.Order.DataAccess.Enums;

namespace Doerly.Module.Order.Domain.Dtos.Responses;
public class GetExecutionProposalResponse
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string? Comment { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public ExecutionProposalStatus Status { get; set; }
}
