using Doerly.Module.Profile.Contracts.Dtos;

namespace Doerly.Module.Communication.Contracts.Responses;

public class ConversationResponse
{
    public int Id { get; set; }
    
    public ProfileDto Initiator { get; set; }
    
    public ProfileDto Recipient { get; set; }
    
    public List<MessageResponse> Messages { get; set; }
}