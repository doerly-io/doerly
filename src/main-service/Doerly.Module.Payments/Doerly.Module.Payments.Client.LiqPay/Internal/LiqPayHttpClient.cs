namespace Doerly.Module.Payments.Client.LiqPay.Client;

public class LiqPayHttpClient
{
    private const string FormUrlEncoding = "application/x-www-form-urlencoded";
    private readonly HttpClient _httpClient;

    public LiqPayHttpClient()
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://www.liqpay.ua/api/"),
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    public async Task<string> CheckoutAsync(string data, string signature)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "3/checkout")
        {
            Content = new StringContent($"data={data}&signature={signature}")
        };

        request.Content.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue(FormUrlEncoding);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var result = response.RequestMessage?.RequestUri?.ToString();

        return result;
    }

    public async Task TransferAsync(string data, string signature)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "/request")
        {
            Content = new StringContent($"data={data}&signature={signature}")
        };

        request.Content.Headers.ContentType =
            new System.Net.Http.Headers.MediaTypeHeaderValue(FormUrlEncoding);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}