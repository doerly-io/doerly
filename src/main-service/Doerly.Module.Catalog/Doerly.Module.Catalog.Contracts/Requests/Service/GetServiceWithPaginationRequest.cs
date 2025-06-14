using Doerly.DataTransferObjects.Pagination;

namespace Doerly.Module.Catalog.Contracts.Requests
{
    public class GetServiceWithPaginationRequest : GetEntitiesWithPaginationRequest
    {
        public int? CategoryId { get; set; }
        public List<FilterValueRequest>? FilterValues { get; set; }
        public string? SortBy { get; set; }
        public string? SearchBy { get; set; }
    }
}
