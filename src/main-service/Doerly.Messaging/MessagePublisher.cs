using Doerly.Domain;
using MassTransit;

namespace Doerly.Messaging;

public class MessagePublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public MessagePublisher(IPublishEndpoint publishEndpoint, IDoerlyRequestContext doerlyRequestContext)
    {
        _publishEndpoint = publishEndpoint;
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
    {
        ArgumentNullException.ThrowIfNull(@event);

        await _publishEndpoint.Publish(@event, context => AddHeaders(context), cancellationToken);
    }

    private void AddHeaders(PublishContext context)
    {
        context.Headers.Set("UserId", _doerlyRequestContext.UserId.ToString());
        context.Headers.Set("UserEmail", _doerlyRequestContext.UserEmail);
        context.Headers.Set("Culture", Thread.CurrentThread.CurrentCulture.Name);
    }
}
