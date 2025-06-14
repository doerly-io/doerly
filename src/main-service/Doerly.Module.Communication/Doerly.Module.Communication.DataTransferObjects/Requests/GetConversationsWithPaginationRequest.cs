namespace Doerly.Module.Communication.DataTransferObjects.Requests;

public class GetConversationsWithPaginationRequest
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}