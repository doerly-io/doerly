using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.Contracts;

public class PaymentHistoryItemResponse
{
    public int PaymentId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public ECurrency Currency { get; set; }
    public EPaymentAction Action { get; set; }
    public EPaymentStatus Status { get; set; }
    public int BillId { get; set; }
    public DateTime CreatedAt { get; set; }
    
}
