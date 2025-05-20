using Doerly.Module.Profile.Contracts.Dtos;

namespace Doerly.Module.Communication.Contracts.Dtos.Responses;

public class ConversationHeaderResponseDto
{
    public int Id { get; set; }
    
    public ProfileDto Initiator { get; set; }
    
    public ProfileDto Recipient { get; set; }
    
    public MessageResponseDto LastMessage { get; set; }
}