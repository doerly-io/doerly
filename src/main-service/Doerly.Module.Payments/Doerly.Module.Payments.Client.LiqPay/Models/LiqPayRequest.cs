namespace Doerly.Module.Payments.Client.LiqPay;

public class LiqPayRequest
{
    public required string Data { get; set; }

    public required string Signature { get; set; }
    
}
