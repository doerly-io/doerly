using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Order.DataTransferObjects.Responses;
public class GetOrdersAmountByCategoriesResponse
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int Amount { get; set; }
}
