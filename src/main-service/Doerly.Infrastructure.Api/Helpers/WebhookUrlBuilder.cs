using Doerly.Common.Settings;
using Microsoft.Extensions.Options;

namespace Doerly.Infrastructure.Api;

public class WebhookUrlBuilder
{
    private readonly IOptions<BackendSettings> _backendSettings;

    public WebhookUrlBuilder(
        IOptions<BackendSettings> backendSettings)
    {
        _backendSettings = backendSettings;
    }

    public Uri BuildWebhookUrl(string partialUrl)
    {
        //ToDo after adding ingress controller replace with passing headers of original request scheme and host
        var url = new Uri(_backendSettings.Value.PublicUrl + partialUrl);

        return url;
    }
}
