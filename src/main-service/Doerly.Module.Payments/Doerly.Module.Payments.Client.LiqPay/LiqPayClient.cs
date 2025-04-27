using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Doerly.Module.Payments.Client.LiqPay;

namespace Doerly.Module.Payments.Client.LiqPay;

public class LiqPayClient : ILiqPayClient
{
    private readonly string _publicKey;
    private readonly string _privateKey;

    public LiqPayClient(string publicKey, string privateKey)
    {
        _publicKey = publicKey;
        _privateKey = privateKey;

        ArgumentException.ThrowIfNullOrEmpty(publicKey);
        ArgumentException.ThrowIfNullOrEmpty(privateKey);
    }

    public LiqPayRequest Checkout(CheckoutRequest checkoutRequest)
    {
        var serializedRequest = JsonSerializer.Serialize(checkoutRequest);
        var base64Request = Convert.ToBase64String(Encoding.UTF8.GetBytes(serializedRequest));
        var signature = GenerateSignature(base64Request);
        var request = new LiqPayRequest
        {
            Data = base64Request,
            Signature = signature
        };

        return request;
    }

    public CheckoutResponse ValidateSignature(string data, string signature)
    {
        var expectedSignature = GenerateSignature(data);
        if (signature != expectedSignature)
            throw new InvalidOperationException("Invalid signature");

        var decodedData = Convert.FromBase64String(data);
        var jsonData = Encoding.UTF8.GetString(decodedData);
        var checkoutResponse = JsonSerializer.Deserialize<CheckoutResponse>(jsonData);

        return checkoutResponse;
    }

    private string GenerateSignature(string base64Request)
    {
        var data = $"{_privateKey}{base64Request}{_privateKey}";
        var hash = SHA1.HashData(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash);
    }
}
