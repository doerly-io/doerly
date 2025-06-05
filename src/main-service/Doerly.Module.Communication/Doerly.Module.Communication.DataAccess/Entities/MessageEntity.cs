using Doerly.DataAccess.Models;
using Doerly.Module.Communication.Enums;

namespace Doerly.Module.Communication.DataAccess.Entities;

public class MessageEntity : BaseEntity
{
    public int ConversationId { get; set; }
    
    public virtual ConversationEntity Conversation { get; set; }
    
    public EMessageType MessageType { get; set; }
    
    public int SenderId { get; set; }
    
    public string MessageContent { get; set; }
    
    public DateTime SentAt { get; set; }
    
    public MessageStatus Status { get; set; }
}