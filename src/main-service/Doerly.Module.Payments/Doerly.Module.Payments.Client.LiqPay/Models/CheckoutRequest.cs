using System.Text.Json.Serialization;

namespace Doerly.Module.Payments.Client.LiqPay;

public class CheckoutRequest
{
    
    [JsonPropertyName("version")]
    public required string ApiVersion { get; set; } = Config.DefaultLiqPayApiVersion;
    
    [JsonPropertyName("public_key")]
    public required string PublicKey { get; set; }

    [JsonPropertyName("action")]
    public required string PaymentAction { get; set; }

    [JsonPropertyName("amount")]
    public required decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public required string Currency { get; set; }
    
    [JsonPropertyName("description")]
    public required string Description { get; set; }
    
    [JsonPropertyName("order_id")]
    public required string OrderId { get; set; }
    
    [JsonPropertyName("result_url")]
    public string ResultUrl { get; set; }

    [JsonPropertyName("server_url")]
    public string ServerUrl { get; set; }
    
}
