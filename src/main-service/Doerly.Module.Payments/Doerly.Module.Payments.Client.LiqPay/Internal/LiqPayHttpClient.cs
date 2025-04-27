using Doerly.Domain.Models;
using Doerly.Module.Payments.Client.LiqPay;

namespace Doerly.Module.Payments.Client.LiqPay.Client;

internal class LiqPayHttpClient
{
    private readonly HttpClient _httpClient;

    internal LiqPayHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    
}
