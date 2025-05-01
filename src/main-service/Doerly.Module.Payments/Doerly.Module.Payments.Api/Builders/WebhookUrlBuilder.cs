using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Doerly.Module.Payments.Api.Builders;

public class WebhookUrlBuilder
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WebhookUrlBuilder(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    public Uri BuildWebhookUrl(string controllerName, string actionName)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var urlString = _linkGenerator.GetUriByAction(
            httpContext,
            action: actionName,
            controller: controllerName,
            values: null,
            scheme: httpContext.Request.Scheme,
            host: httpContext.Request.Host);

        var url = new Uri(urlString);
        return url;
    }
}