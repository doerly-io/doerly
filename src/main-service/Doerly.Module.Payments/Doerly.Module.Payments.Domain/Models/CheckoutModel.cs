namespace Doerly.Module.Payments.Domain.Models;

public class CheckoutModel
{
    public required string PaymentAction { get; set; }
    public required decimal Amount { get; set; }
    public required string Currency { get; set; }
    public required string Description { get; set; }
    public required string BillId { get; set; }
    public string ResultUrl { get; set; }
    public string ServerUrl { get; set; }
    public string ApiVersion { get; set; }
}
