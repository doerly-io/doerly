using Doerly.Module.Communication.Enums;
using Doerly.Module.Profile.Contracts.Dtos;

namespace Doerly.Module.Communication.Contracts.Responses;

public class MessageResponse
{
    public int Id { get; set; }
    
    public int ConversationId { get; set; }
    
    public ConversationHeaderResponse Conversation { get; set; }
    
    public EMessageType MessageType { get; set; }
    
    public int SenderId { get; set; }
    
    public ProfileDto Sender { get; set; }
    
    public string MessageContent { get; set; }
    
    public DateTime SentAt { get; set; }
    
    public EMessageStatus Status { get; set; }
}