namespace Doerly.Module.Communication.Contracts.Dtos.Requests;

public class GetConversationsWithPaginationRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}