using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.DataTransferObjects.Pagination;

namespace Doerly.Module.Order.DataTransferObjects.Dtos;
public class GetOrdersWithPaginationRequest : GetEntitiesWithPaginationRequest
{ 
    public int? CustomerId { get; set; }

    public int? ExecutorId { get; set; }
}
