using Doerly.Domain.Models;

using Microsoft.AspNetCore.Diagnostics;

namespace Doerly.Host.ExceptionHandlers;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Exception occurred: {Message}",
            exception.Message);

        var OperationResult = new OperationResult<object>(false, exception.Message);

        httpContext.Response.StatusCode = 500;

        await httpContext.Response
            .WriteAsJsonAsync(OperationResult, cancellationToken);

        return true;
    }
}
