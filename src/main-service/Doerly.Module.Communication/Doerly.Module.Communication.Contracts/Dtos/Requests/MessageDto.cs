using Doerly.Module.Communication.Enums;

namespace Doerly.Module.Communication.Contracts.Dtos.Requests;

public class MessageDto
{
    public int Id { get; set; }
    
    public int ConversationId { get; set; }
    
    public virtual ConversationDto Conversation { get; set; }
    
    public MessageType MessageType { get; set; }
    
    public int SenderId { get; set; }
    
    public string MessageContent { get; set; }
    
    public DateTime SentAt { get; set; }
    
    public MessageStatus Status { get; set; }
}