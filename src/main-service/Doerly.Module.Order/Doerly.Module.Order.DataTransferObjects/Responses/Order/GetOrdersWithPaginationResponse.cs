namespace Doerly.Module.Order.DataTransferObjects.Responses;
public class GetOrdersWithPaginationResponse
{
    public int Total { get; set; }

    public List<GetOrderResponse> Orders { get; set; } = new List<GetOrderResponse>();
}
