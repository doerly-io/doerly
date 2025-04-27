using Doerly.DataAccess.Models;

namespace Doerly.Module.Payments.DataAccess.Models;

public class Invoice : BaseEntity
{
    /// <summary>
    /// Amount to be paid by customer
    /// </summary>
    public decimal AmountTotal { get; set; }

    /// <summary>
    /// Amount already paid
    /// </summary>
    public decimal AmountPaid { get; set; }

    /// <summary>
    /// Invoice description
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Payment transactions made for this invoice
    /// </summary>
    public virtual ICollection<Payment> Payments { get; set; }
    
}
