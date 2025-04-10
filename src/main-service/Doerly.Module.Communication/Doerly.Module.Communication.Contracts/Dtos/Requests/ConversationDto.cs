namespace Doerly.Module.Communication.Contracts.Dtos.Requests;

public class ConversationDto
{
    public int InitiatorId { get; set; }
    
    public int RecipientId { get; set; }
    
    public int? LastMessageId { get; set; }
}