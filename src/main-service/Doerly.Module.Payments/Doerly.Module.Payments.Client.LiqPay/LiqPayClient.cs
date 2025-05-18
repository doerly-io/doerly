using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Doerly.Module.Payments.BaseClient;
using Doerly.Module.Payments.Client.LiqPay.Client;
using Doerly.Module.Payments.Client.LiqPay.Internal;
using Doerly.Module.Payments.Contracts;
using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.Client.LiqPay;

public class LiqPayClient : IBasePaymentClient
{
    private readonly string _publicKey;
    private readonly string _privateKey;
    private readonly LiqPayHttpClient _httpClient;

    private readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public LiqPayClient(string publicKey, string privateKey, LiqPayHttpClient httpClient)
    {
        _publicKey = publicKey;
        _privateKey = privateKey;
        _httpClient = httpClient;

        ArgumentException.ThrowIfNullOrEmpty(publicKey);
        ArgumentException.ThrowIfNullOrEmpty(privateKey);
    }

    public async Task<BaseCheckoutResponse> Checkout(CheckoutModel checkoutModel)
    {
        var dto = checkoutModel.ToDto(_publicKey);
        var serializedRequest = JsonSerializer.Serialize(dto);
        var base64Request = Convert.ToBase64String(Encoding.UTF8.GetBytes(serializedRequest));
        var signature = GenerateSignature(base64Request);

        var checkoutUrl = await _httpClient.CheckoutAsync(base64Request, signature);
        var result = new BaseCheckoutResponse
        {
            CheckoutUrl = checkoutUrl,
            BillId = checkoutModel.BillId,
            Aggregator = EPaymentAggregator.LiqPay
        };

        return result;
    }

    public bool ValidateSignature(string data, string signature)
    {
        var expectedSignature = GenerateSignature(data);
        return signature == expectedSignature;
    }

    public string GenerateSignature(string data)
    {
        var signatureData = $"{_privateKey}{data}{_privateKey}";
        var hash = SHA1.HashData(Encoding.UTF8.GetBytes(signatureData));
        return Convert.ToBase64String(hash);
    }
}
