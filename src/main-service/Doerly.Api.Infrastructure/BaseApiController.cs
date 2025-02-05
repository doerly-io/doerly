using System.Diagnostics;
using Doerly.Domain.Factories;
using Doerly.Domain.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Doerly.Api.Infrastructure;

public class BaseApiController : ControllerBase
{
    [DebuggerStepThrough]
    protected THandler ResolveHandler<THandler>() where THandler : IHandler
    {
        var handlerFactory = HttpContext.RequestServices.GetRequiredService<IHandlerFactory>();
        return handlerFactory.Get<THandler>();
    }

    [NonAction]
    protected void SetHttpCookie(string key, string value)
    {
        Response.Cookies.Append(key, value, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Lax
        });
    }

    [NonAction]
    protected string GetBearerToken()
    {
        var authorizationHeader = Request.Headers["Authorization"].ToString();
        return authorizationHeader.Replace("Bearer ", "");
    }
}
