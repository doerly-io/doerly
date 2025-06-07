using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Order.Contracts.Dtos;
public class GetOrdersWithPaginationResponse
{
    public int Total { get; set; }

    public List<GetOrderResponse> Orders { get; set; } = new List<GetOrderResponse>();
}
