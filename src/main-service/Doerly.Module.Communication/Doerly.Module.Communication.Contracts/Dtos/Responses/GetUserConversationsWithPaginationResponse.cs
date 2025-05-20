namespace Doerly.Module.Communication.Contracts.Dtos.Responses;

public class GetUserConversationsWithPaginationResponse
{
    public int Total { get; set; }

    public List<ConversationHeaderResponseDto> Conversations { get; set; } = [];
}