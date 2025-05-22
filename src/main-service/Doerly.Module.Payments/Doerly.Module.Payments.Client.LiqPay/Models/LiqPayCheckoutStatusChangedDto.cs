using System.Text.Json.Serialization;

namespace Doerly.Module.Payments.Client.LiqPay;

public class LiqPayCheckoutStatusChangedDto
{
    [JsonPropertyName("payment_id")]
    public long PaymentId { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("version")]
    public int ApiVersion { get; set; }

    [JsonPropertyName("type")]
    public string OperationType { get; set; }

    [JsonPropertyName("paytype")]
    public string PaymentMethod { get; set; }

    [JsonPropertyName("public_key")]
    public string PublicKey { get; set; }

    [JsonPropertyName("acq_id")]
    public int AcquirerId { get; set; }

    [JsonPropertyName("order_id")]
    public string OrderId { get; set; }

    [JsonPropertyName("liqpay_order_id")]
    public string LiqPayOrderId { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("sender_first_name")]
    public string SenderFirstName { get; set; }

    [JsonPropertyName("sender_last_name")]
    public string SenderLastName { get; set; }

    [JsonPropertyName("sender_card_mask2")]
    public string SenderCardMask { get; set; }

    [JsonPropertyName("sender_card_bank")]
    public string SenderCardBank { get; set; }

    [JsonPropertyName("sender_card_type")]
    public string SenderCardType { get; set; }

    [JsonPropertyName("sender_card_country")]
    public int SenderCardCountryCode { get; set; }

    [JsonPropertyName("ip")]
    public string SenderIp { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("sender_commission")]
    public decimal SenderCommission { get; set; }

    [JsonPropertyName("receiver_commission")]
    public decimal ReceiverCommission { get; set; }

    [JsonPropertyName("agent_commission")]
    public decimal AgentCommission { get; set; }

    [JsonPropertyName("amount_debit")]
    public decimal AmountDebited { get; set; }

    [JsonPropertyName("amount_credit")]
    public decimal AmountCredited { get; set; }

    [JsonPropertyName("commission_debit")]
    public decimal CommissionDebited { get; set; }

    [JsonPropertyName("commission_credit")]
    public decimal CommissionCredited { get; set; }

    [JsonPropertyName("currency_debit")]
    public string CurrencyDebited { get; set; }

    [JsonPropertyName("currency_credit")]
    public string CurrencyCredited { get; set; }

    [JsonPropertyName("sender_bonus")]
    public decimal SenderBonus { get; set; }

    [JsonPropertyName("amount_bonus")]
    public decimal AmountBonus { get; set; }

    [JsonPropertyName("mpi_eci")]
    public string MpiEci { get; set; }

    [JsonPropertyName("is_3ds")]
    public bool Is3DS { get; set; }

    [JsonPropertyName("language")]
    public string Language { get; set; }

    [JsonPropertyName("create_date")]
    public long CreateDateUnix { get; set; }

    [JsonPropertyName("end_date")]
    public long EndDateUnix { get; set; }

    [JsonPropertyName("transaction_id")]
    public long TransactionId { get; set; }
}


