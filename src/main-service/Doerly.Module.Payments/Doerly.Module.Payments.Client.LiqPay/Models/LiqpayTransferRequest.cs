using System.Text.Json.Serialization;

namespace Doerly.Module.Payments.Client.LiqPay;

public class LiqpayTransferRequest
{
    [JsonPropertyName("version")]
    public int Version { get; set; } = 3;

    [JsonPropertyName("public_key")]
    public string PublicKey { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("ip")]
    public string Ip { get; set; }

    [JsonPropertyName("order_id")]
    public string OrderId { get; set; }
    
    [JsonPropertyName("receiver_card")]
    public string ReceiverCard { get; set; }
}