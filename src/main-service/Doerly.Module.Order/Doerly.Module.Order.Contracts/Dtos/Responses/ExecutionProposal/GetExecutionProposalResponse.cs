using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.Contracts.Dtos;
public class GetExecutionProposalResponse
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string Comment { get; set; }

    public int SenderId { get; set; }

    public ProfileInfo Sender { get; set; }

    public int ReceiverId { get; set; }

    public ProfileInfo Receiver { get; set; }

    public EExecutionProposalStatus Status { get; set; }
}
