using Doerly.Domain.Factories;
using Doerly.Module.Authorization.Domain.Handlers;
using Doerly.Module.Authorization.Contracts.Messages;
using MassTransit;

namespace Doerly.Module.Authorization.Domain.EventConsumers;

public class PasswordVerificationRequestedEventConsumer : IConsumer<PasswordResetRequestMessage>
{
    private readonly IHandlerFactory _handlerFactory;

    public PasswordVerificationRequestedEventConsumer(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    public async Task Consume(ConsumeContext<PasswordResetRequestMessage> context)
    {
        await _handlerFactory.Get<RequestPasswordResetHandler>().HandleAsync(context.Message.Email);
    }
}
