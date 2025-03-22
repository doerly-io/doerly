using Doerly.DataAccess.Models;
using Doerly.Module.Communication.Enums;
using Doerly.Module.Profile.DataAccess.Models;

namespace Doerly.Module.Communication.DataAccess.Entities;

public class MessageEntity : BaseEntity
{
    public int ConversationId { get; set; }
    
    public virtual ConversationEntity Conversation { get; set; }
    
    public MessageType MessageType { get; set; }
    
    public int SenderId { get; set; }
    
    public virtual ProfileEntity Sender { get; set; }
    
    public string MessageContent { get; set; }
    
    public DateTime SentAt { get; set; }
    
    public MessageStatus Status { get; set; }
}