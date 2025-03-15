using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Order.Domain.Dtos.Responses;
public class GetOrdersWithPaginationResponse
{
    public int Total { get; set; }

    public List<GetOrderResponse> Orders { get; set; } = new List<GetOrderResponse>();
}
