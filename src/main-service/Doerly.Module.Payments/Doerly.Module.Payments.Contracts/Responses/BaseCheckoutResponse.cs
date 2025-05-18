using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.Contracts;

public class BaseCheckoutResponse
{
    public EPaymentAggregator Aggregator { get; set; }
    
    public int BillId { get; set; }
    
    public string CheckoutUrl { get; set; }
}
