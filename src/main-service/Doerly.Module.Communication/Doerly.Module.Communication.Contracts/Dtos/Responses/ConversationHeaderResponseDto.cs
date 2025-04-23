namespace Doerly.Module.Communication.Contracts.Dtos.Responses;

public class ConversationHeaderResponseDto
{
    public int Id { get; set; }
    
    public int InitiatorId { get; set; }
    
    public int RecipientId { get; set; }
}