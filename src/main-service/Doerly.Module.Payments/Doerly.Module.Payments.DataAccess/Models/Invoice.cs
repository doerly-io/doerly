using Doerly.DataAccess.Models;

namespace Doerly.Module.Payments.DataAccess.Models;

public class Invoice : BaseEntity
{
    public decimal AmountTotal { get; set; }

    public decimal AmountPaid { get; set; }
    
    public virtual ICollection<Payment> Payments { get; set; }
    
}
