using Doerly.DataTransferObjects.Pagination;

namespace Doerly.Module.Catalog.Contracts.Requests
{
    public class GetServiceWithPaginationRequest : GetEntitiesWithPaginationRequest
    {
        public int? CategoryId { get; set; }
        public Dictionary<int, string>? FilterValues { get; set; }
        public string? SortBy { get; set; }
    }
}
