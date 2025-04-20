using MassTransit;

namespace Doerly.Messaging;

public class MessagePublisher : IMessagePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public MessagePublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class
    {
        ArgumentNullException.ThrowIfNull(@event);

        await _publishEndpoint.Publish(@event, cancellationToken);
    }
}
