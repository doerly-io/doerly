using Doerly.Domain.Factories;
using Doerly.Messaging;
using Doerly.Module.Authorization.Domain.Handlers;
using Doerly.Module.Authorization.Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Authorization.Domain.EventConsumers;

public class PasswordVerificationRequestedEventConsumer : BaseConsumer<PasswordResetRequestMessage>
{
    private readonly IHandlerFactory _handlerFactory;

    public PasswordVerificationRequestedEventConsumer(ILogger<PasswordVerificationRequestedEventConsumer> logger, IHandlerFactory handlerFactory) : base(logger)
    {
        _handlerFactory = handlerFactory;
    }

    protected override async Task Handle(ConsumeContext<PasswordResetRequestMessage> context)
    {
        await _handlerFactory.Get<RequestPasswordResetHandler>().HandleAsync(context.Message.Email);
    }
}
