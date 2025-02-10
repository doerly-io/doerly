using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Order.Domain.Dtos.Requests.Order;
public class GetOrderHistoryRequest
{
	public int PageNumber { get; set; }
	public int PageSize { get; set; }
}
