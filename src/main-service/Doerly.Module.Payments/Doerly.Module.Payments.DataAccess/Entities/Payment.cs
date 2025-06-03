using Doerly.DataAccess.Models;
using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.DataAccess.Models;

public class Payment : BaseEntity
{
    public Guid Guid { get; set; }
    public decimal Amount { get; set; }

    public required string Description { get; set; }

    public ECurrency Currency { get; set; }

    public EPaymentAction Action { get; set; }
    
    public EPaymentStatus Status { get; set; }

    public string? CheckoutUrl { get; set; }
    
    public int BillId { get; set; }
    
    public virtual Bill Bill { get; set; }
}
