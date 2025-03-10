using Doerly.DataAccess.Models;
using Doerly.Module.Payments.DataAccess.Enums;

namespace Doerly.Module.Payments.DataAccess.Models;

public class Payment : BaseEntity
{
    public decimal Amount { get; set; }

    public ECurrency Currency { get; set; }

    public EPaymentMethod PaymentMethod { get; set; }
    
    public int InvoiceId { get; set; }
    
    public virtual Invoice Invoice { get; set; }
}
