using Doerly.Domain.Exceptions;
using Doerly.Domain.Models;

using Microsoft.AspNetCore.Diagnostics;

namespace Doerly.Host.ExceptionHandlers;

internal sealed class DoerlyExceptionHandler : IExceptionHandler
{
    private readonly ILogger<DoerlyExceptionHandler> _logger;

    public DoerlyExceptionHandler(ILogger<DoerlyExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DoerlyException doerlyException)
        {
            return false;
        }

        _logger.LogError(
            doerlyException,
            "Exception occurred: {Message}",
            doerlyException.Message);

        var handlerResult = new HandlerResult<object>(false, doerlyException.Message);

        httpContext.Response.StatusCode = 400;

        await httpContext.Response.WriteAsJsonAsync(handlerResult, cancellationToken);

        return true;
    }
}
