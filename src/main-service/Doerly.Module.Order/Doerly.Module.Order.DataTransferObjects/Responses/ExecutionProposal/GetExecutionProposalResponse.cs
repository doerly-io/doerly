using Doerly.Module.Order.DataTransferObjects;
using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.DataTransferObjects;
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

    public DateTime DateCreated { get; set; }
}
