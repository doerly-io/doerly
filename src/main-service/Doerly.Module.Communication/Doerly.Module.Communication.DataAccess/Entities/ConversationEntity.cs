using Doerly.DataAccess.Models;
using Doerly.Module.Profile.DataAccess.Models;

namespace Doerly.Module.Communication.DataAccess.Entities;

public class ConversationEntity : BaseEntity
{
    public string ConversationName { get; set; }
    
    public int InitiatorId { get; set; }
    public virtual ProfileEntity Initiator { get; set; }
    
    public int RecipientId { get; set; }
    public virtual ProfileEntity Recipient { get; set; }
    
    public int? LastMessageId { get; set; }
    
    public virtual MessageEntity LastMessage { get; set; }
    public virtual ICollection<MessageEntity> Messages { get; set; }
}