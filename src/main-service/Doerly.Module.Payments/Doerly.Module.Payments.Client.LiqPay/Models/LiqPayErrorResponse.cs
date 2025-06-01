using System.Text.Json.Serialization;

namespace Doerly.Module.Payments.Client.LiqPay;

public class LiqPayErrorResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("err_code")]
    public string ErrorCode { get; set; }

    [JsonPropertyName("err_description")]
    public string ErrorDescription { get; set; }
}
