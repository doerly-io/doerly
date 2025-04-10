using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Communication.Contracts.Dtos.Requests;

public class SendMessageDto
{
    public int? ConversationId { get; set; }
    
    public required int InitiatorId { get; set; }
    
    public required int RecipientId { get; set; }
    
    public required string MessageContent { get; set; }
}