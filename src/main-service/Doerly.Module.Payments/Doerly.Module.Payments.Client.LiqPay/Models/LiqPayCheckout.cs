namespace Doerly.Module.Payments.Client.LiqPay;

public class LiqPayCheckout
{
    public required string PaymentAction { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required string Description { get; set; }
    public required string OrderId { get; set; }
    public string ResultUrl { get; set; }
    public string ServerUrl { get; set; }
    public string ApiVersion { get; set; } = Config.DefaultLiqPayApiVersion;
}
