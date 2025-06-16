namespace Doerly.Module.Payments.BaseClient;

public class TransferModel
{
    public string PublicKey { get; set; }

    public string Action { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; }

    public string Description { get; set; }

    //public string Ip { get; set; }

    public string PaymentGuid { get; set; }
    
    public string ReceiverCard { get; set; }
}