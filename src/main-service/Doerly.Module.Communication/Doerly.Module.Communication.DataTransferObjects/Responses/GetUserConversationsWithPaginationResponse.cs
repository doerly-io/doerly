namespace Doerly.Module.Communication.DataTransferObjects.Responses;

public class GetUserConversationsWithPaginationResponse
{
    public int Total { get; set; }

    public List<ConversationHeaderResponse> Conversations { get; set; } = [];
}