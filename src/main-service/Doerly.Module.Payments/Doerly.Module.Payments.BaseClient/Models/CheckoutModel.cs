using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.BaseClient;

public class CheckoutModel
{
    public required EPaymentAction PaymentAction { get; set; }
    public required decimal Amount { get; set; }
    public required ECurrency Currency { get; set; }
    public required string Description { get; set; }
    public required string PaymentId { get; set; }
    public required int BillId { get; set; }
    public string ReturnUrl { get; set; }
    public string CallbackUrl { get; set; }
    public string ApiVersion { get; set; }
    public DateTime ExpireDate { get; set; }
}