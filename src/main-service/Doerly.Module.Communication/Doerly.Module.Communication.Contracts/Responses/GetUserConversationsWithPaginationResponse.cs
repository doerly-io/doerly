namespace Doerly.Module.Communication.Contracts.Responses;

public class GetUserConversationsWithPaginationResponse
{
    public int Total { get; set; }

    public List<ConversationHeaderResponse> Conversations { get; set; } = [];
}