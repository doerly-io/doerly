namespace Doerly.Module.Communication.DataTransferObjects.Requests;

public class SendMessageRequest
{
    public required int ConversationId { get; set; }
    
    public required string MessageContent { get; set; }
}