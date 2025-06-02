namespace Doerly.Module.Communication.Contracts.Dtos.Requests;

public class SendMessageRequest
{
    public required int ConversationId { get; set; }
    
    public required string MessageContent { get; set; }
    
    public int SenderId { get; set; }
}