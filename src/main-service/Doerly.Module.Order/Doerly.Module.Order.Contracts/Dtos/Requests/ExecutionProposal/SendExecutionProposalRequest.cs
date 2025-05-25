namespace Doerly.Module.Order.Contracts.Dtos;

public class SendExecutionProposalRequest
{
    public int OrderId { get; set; }

    public string? Comment { get; set; }

    public int SenderId { get; set; }

    public int ReceiverId { get; set; }
}
