using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Module.Order.DataAccess.Enums;

namespace Doerly.Module.Order.Domain.Dtos.Requests.Order;
public class UpdateOrderRequest
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public PaymentKind PaymentKind { get; set; }
    public DateTime DueDate { get; set; }
}
