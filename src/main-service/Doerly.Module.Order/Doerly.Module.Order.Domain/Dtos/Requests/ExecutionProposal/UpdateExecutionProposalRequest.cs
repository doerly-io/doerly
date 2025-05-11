using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Order.Domain.Dtos.Requests;
public class UpdateExecutionProposalRequest
{
    public string? Comment { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
}
