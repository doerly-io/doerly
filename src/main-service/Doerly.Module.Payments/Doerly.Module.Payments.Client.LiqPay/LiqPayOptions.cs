namespace Doerly.Module.Payments.Client.LiqPay;

public class LiqPayOptions
{
    public string PublicKey { get; set; } = string.Empty;
    public string PrivateKey { get; set; } = string.Empty;
    public string Language { get; set; } = Config.DefaultLanguage;
}
