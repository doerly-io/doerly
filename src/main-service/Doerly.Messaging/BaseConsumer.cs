using System.Globalization;
using Doerly.Domain;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Doerly.Messaging;

public abstract class BaseConsumer<TMessage> : IConsumer<TMessage> where TMessage : class
{
    private readonly ILogger _logger;
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    protected BaseConsumer(ILogger logger, IDoerlyRequestContext doerlyRequestContext)
    {
        _logger = logger;
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task Consume(ConsumeContext<TMessage> context)
    {
        try
        {
            _logger.LogInformation("Consuming message of type {MessageType}", nameof(TMessage));

            await BeforeConsume(context);
            await Handle(context);
            await AfterConsume(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while consuming {MessageType}", nameof(TMessage));
            await OnError(context, ex);
            throw;
        }
    }

    protected virtual Task BeforeConsume(ConsumeContext<TMessage> context)
    {
        _doerlyRequestContext.UserEmail = context.Headers.Get("UserEmail", string.Empty);
        if (int.TryParse(context.Headers.Get("UserId", string.Empty), out var userId))
            _doerlyRequestContext.UserId = userId;
        
        var cultureName = context.Headers.Get<string>("Culture") ?? "en-US";
        var culture = new CultureInfo(cultureName);

        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        
        return Task.CompletedTask;  
    }

    protected virtual Task AfterConsume(ConsumeContext<TMessage> context) => Task.CompletedTask;
    protected virtual Task OnError(ConsumeContext<TMessage> context, Exception exception) => Task.CompletedTask;

    protected abstract Task Handle(ConsumeContext<TMessage> context);
}
