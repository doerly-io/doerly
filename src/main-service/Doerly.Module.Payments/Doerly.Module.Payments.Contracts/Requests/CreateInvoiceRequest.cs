using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Payments.Contracts.Requests;

public class CreateInvoiceRequest
{
    public Guid OrderGuid { get; set; }
    
    public decimal AmountTotal { get; set; }

    /// <summary>
    /// Id of user who will receive the payment.
    /// </summary>
    public int ExecutorId { get; set; }

    [MinLength(5)]
    [MaxLength(50)]
    public string Description { get; set; }
    
}
