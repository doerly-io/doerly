using Doerly.DataAccess.Models;
using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.DataAccess.Models;

public class Order : BaseEntity
{
    public int CategoryId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public EPaymentKind PaymentKind { get; set; }

    public DateTime DueDate { get; set; }

    public EOrderStatus Status { get; set; }

    public int CustomerId { get; set; }

    public bool CustomerCompletionConfirmed { get; set; }

    public int? ExecutorId { get; set; }

    public bool ExecutorCompletionConfirmed { get; set; }

    public DateTime? ExecutionDate { get; set; }

    public int? BillId { get; set; }

    public virtual ICollection<ExecutionProposal> ExecutionProposals { get; set; }
}
