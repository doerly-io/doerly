using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Payments.Api;

public class PaymentSettings
{
    public const string PaymentSettingsName = nameof(PaymentSettings);

    public LiqPaySettings LiqPay { get; set; }

    public class LiqPaySettings
    {
        [Required]
        public string PublicKey { get; set; }

        [Required]
        public string PrivateKey { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string ApiUrl { get; set; }
    }

    // [Required]
    // public MonoplataSettings Monoplata { get; set; }
    //
    // public class MonoplataSettings
    // {
    //     [Required]
    //     public string Token { get; set; }
    //     
    //     [Required]
    //     [DataType(DataType.Url)]
    //     public string ApiUrl { get; set; }
    // }
}
