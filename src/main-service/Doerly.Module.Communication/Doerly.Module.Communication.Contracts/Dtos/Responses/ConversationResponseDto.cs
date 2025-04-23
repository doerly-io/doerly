using Doerly.Module.Communication.Contracts.Dtos.Requests;

namespace Doerly.Module.Communication.Contracts.Dtos.Responses;

public class ConversationResponseDto
{
    public int Id { get; set; }
    
    public int InitiatorId { get; set; }
    
    public int RecipientId { get; set; }
    
    
    public int? LastMessageId { get; set; }
    
    public List<MessageResponseDto> Messages { get; set; }
}