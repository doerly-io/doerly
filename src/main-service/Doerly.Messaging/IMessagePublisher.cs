namespace Doerly.Messaging;

public interface IMessagePublisher
{
    Task Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class;
}
