namespace Doerly.Module.Communication.Contracts.Requests;

public class SendMessageRequest
{
    public required int ConversationId { get; set; }
    
    public required string MessageContent { get; set; }
}