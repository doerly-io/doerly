using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.Contracts.Dtos;
public class UpdateOrderRequest
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public EPaymentKind PaymentKind { get; set; }
    public DateTime DueDate { get; set; }
}
