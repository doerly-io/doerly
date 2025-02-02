using Doerly.DataAccess.Models;
using Doerly.Module.Order.DataAccess.Constants;
using Doerly.Module.Order.DataAccess.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Doerly.Module.Order.DataAccess.Models;

[Table(DbConstants.Tables.Order, Schema = DbConstants.OrderSchema)]
public class Order : BaseEntity
{
    public int CategoryId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public PaymentKind PaymentKind { get; set; }

    public DateTime DueDate { get; set; }

    public OrderStatus Status { get; set; }

    public int CustomerId { get; set; }
    
    public int? ExecutorId { get; set; }

    public DateTime? ExecutionDate { get; set; }

    public int? BillId { get; set; }

    public virtual ICollection<ExecutionProposal> ExecutionProposals { get; set; }
}
