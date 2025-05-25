namespace Doerly.Module.Order.Contracts.Dtos;
public class UpdateExecutionProposalRequest
{
    public string? Comment { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
}
