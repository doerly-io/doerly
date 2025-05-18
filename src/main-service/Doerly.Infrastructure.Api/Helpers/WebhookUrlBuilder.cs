using Doerly.Common.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Doerly.Infrastructure.Api;

public class WebhookUrlBuilder
{
    private readonly IOptions<BackendSettings> _backendSettings;
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WebhookUrlBuilder(
        IOptions<BackendSettings> backendSettings,
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor)
    {
        _backendSettings = backendSettings;
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    public Uri BuildWebhookUrl(string controllerName, string actionName)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        
        //ToDo after adding ingress controller replace with passing headers of original request scheme and host
        var baseUrl = new Uri(_backendSettings.Value.PublicUrl); 

        var urlString = _linkGenerator.GetUriByAction(
            httpContext,
            action: actionName,
            controller: controllerName,
            values: null,
            scheme: baseUrl.Scheme,
            host: new HostString(baseUrl.Authority));

        var url = new Uri(urlString);
        return url;
    }
}
