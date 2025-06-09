using Doerly.Module.Profile.DataTransferObjects;

namespace Doerly.Module.Communication.Contracts.Responses;

public class ConversationHeaderResponse
{
    public int Id { get; set; }
    
    public ProfileDto Initiator { get; set; }
    
    public ProfileDto Recipient { get; set; }
    
    public MessageResponse? LastMessage { get; set; }
}