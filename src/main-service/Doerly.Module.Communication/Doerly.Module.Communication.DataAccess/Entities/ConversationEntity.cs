using Doerly.DataAccess.Models;

namespace Doerly.Module.Communication.DataAccess.Entities;

public class ConversationEntity : BaseEntity
{
    public string ConversationName { get; set; }
    
    public int InitiatorId { get; set; }
    
    public int RecipientId { get; set; }
    
    public int? LastMessageId { get; set; }
    
    public IEnumerable<int> ParticipantIds => [InitiatorId, RecipientId];
    
    public virtual ICollection<MessageEntity> Messages { get; set; }
}