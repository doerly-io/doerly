using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Module.Order.DataAccess.Enums;

namespace Doerly.Module.Order.Domain.Dtos.Responses;
public class GetOrderResponse
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public EPaymentKind PaymentKind { get; set; }
    public DateTime DueDate { get; set; }
    public EOrderStatus Status { get; set; }
    public int CustomerId { get; set; }
    public int? ExecutorId { get; set; }
    public DateTime? ExecutionDate { get; set; }
    public int? BillId { get; set; }
}
