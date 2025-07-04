using Doerly.DataTransferObjects.Pagination;

namespace Doerly.Module.Order.DataTransferObjects.Requests;

public class GetOrderWithFilterAndPaginationRequest : GetEntitiesWithPaginationRequest
{
    public int? CategoryId { get; set; }
    
    public bool IsOrderByPrice { get; set; }

    public bool IsDescending { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public string? SearchValue { get; set; }

    public int? RegionId { get; set; }
}
