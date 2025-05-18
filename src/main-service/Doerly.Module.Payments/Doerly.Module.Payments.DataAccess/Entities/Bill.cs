using Doerly.DataAccess.Models;

namespace Doerly.Module.Payments.DataAccess.Models;

public class Bill : BaseEntity
{
    public int PayerId { get; set; }
    
    /// <summary>
    /// Amount to be paid by customer
    /// </summary>
    public decimal AmountTotal { get; set; }

    /// <summary>
    /// Amount already paid
    /// </summary>
    public decimal AmountPaid { get; set; }

    /// <summary>
    /// Bill description
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Payment transactions made for this invoice
    /// </summary>
    public virtual ICollection<Payment> Payments { get; set; }
    
}
