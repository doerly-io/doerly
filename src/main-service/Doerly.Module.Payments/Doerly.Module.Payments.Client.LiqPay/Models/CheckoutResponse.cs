using System.Text.Json.Serialization;

namespace Doerly.Module.Payments.Client.LiqPay;

public class CheckoutResponse
{
    [JsonPropertyName("acq_id")]
    public int AcqId { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("agent_commission")]
    public decimal AgentCommission { get; set; }

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("amount_bonus")]
    public decimal AmountBonus { get; set; }

    [JsonPropertyName("amount_credit")]
    public decimal AmountCredit { get; set; }

    [JsonPropertyName("amount_debit")]
    public decimal AmountDebit { get; set; }

    [JsonPropertyName("authcode_credit")]
    public string AuthcodeCredit { get; set; }

    [JsonPropertyName("authcode_debit")]
    public string AuthcodeDebit { get; set; }

    [JsonPropertyName("card_token")]
    public string CardToken { get; set; }

    [JsonPropertyName("commission_credit")]
    public decimal CommissionCredit { get; set; }

    [JsonPropertyName("commission_debit")]
    public decimal CommissionDebit { get; set; }

    [JsonPropertyName("completion_date")]
    public string CompletionDate { get; set; }

    [JsonPropertyName("create_date")]
    public string CreateDate { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    [JsonPropertyName("currency_credit")]
    public string CurrencyCredit { get; set; }

    [JsonPropertyName("currency_debit")]
    public string CurrencyDebit { get; set; }

    [JsonPropertyName("customer")]
    public string Customer { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("end_date")]
    public string EndDate { get; set; }

    [JsonPropertyName("err_code")]
    public string ErrCode { get; set; }

    [JsonPropertyName("err_description")]
    public string ErrDescription { get; set; }

    [JsonPropertyName("info")]
    public string Info { get; set; }

    [JsonPropertyName("ip")]
    public string Ip { get; set; }

    [JsonPropertyName("is_3ds")]
    public bool Is3ds { get; set; }

    [JsonPropertyName("liqpay_order_id")]
    public string LiqpayOrderId { get; set; }

    [JsonPropertyName("mpi_eci")]
    public int MpiEci { get; set; }

    [JsonPropertyName("order_id")]
    public string OrderId { get; set; }

    [JsonPropertyName("payment_id")]
    public int PaymentId { get; set; }

    [JsonPropertyName("paytype")]
    public string Paytype { get; set; }

    [JsonPropertyName("public_key")]
    public string PublicKey { get; set; }

    [JsonPropertyName("receiver_commission")]
    public decimal ReceiverCommission { get; set; }

    [JsonPropertyName("redirect_to")]
    public string RedirectTo { get; set; }

    [JsonPropertyName("refund_date_last")]
    public string RefundDateLast { get; set; }

    [JsonPropertyName("rrn_credit")]
    public string RrnCredit { get; set; }

    [JsonPropertyName("rrn_debit")]
    public string RrnDebit { get; set; }

    [JsonPropertyName("sender_bonus")]
    public decimal SenderBonus { get; set; }

    [JsonPropertyName("sender_card_bank")]
    public string SenderCardBank { get; set; }

    [JsonPropertyName("sender_card_country")]
    public string SenderCardCountry { get; set; }

    [JsonPropertyName("sender_card_mask2")]
    public string SenderCardMask2 { get; set; }

    [JsonPropertyName("sender_card_type")]
    public string SenderCardType { get; set; }

    [JsonPropertyName("sender_commission")]
    public decimal SenderCommission { get; set; }

    [JsonPropertyName("sender_first_name")]
    public string SenderFirstName { get; set; }

    [JsonPropertyName("sender_last_name")]
    public string SenderLastName { get; set; }

    [JsonPropertyName("sender_phone")]
    public string SenderPhone { get; set; }
}
