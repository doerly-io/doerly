using Doerly.DataTransferObjects.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Contracts.Dtos.Requests.Service
{
    public class GetServiceWithPaginationRequest : GetEntitiesWithPaginationRequest
    {
        public int? CategoryId { get; set; }
        public Dictionary<int, string>? FilterValues { get; set; }
        public string? SortBy { get; set; }
    }
}
