using Doerly.Domain;
using Doerly.Domain.Factories;
using Doerly.Messaging;
using Doerly.Module.Authorization.Domain.Handlers;
using Doerly.Module.Authorization.DataTransferObjects.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Authorization.Domain.EventConsumers;

public class PasswordVerificationRequestedEventConsumer : BaseConsumer<PasswordResetRequestMessage>
{
    private readonly IHandlerFactory _handlerFactory;

    public PasswordVerificationRequestedEventConsumer(
        ILogger<PasswordVerificationRequestedEventConsumer> logger,
        IDoerlyRequestContext doerlyRequestContext,
        IHandlerFactory handlerFactory) : base(logger, doerlyRequestContext)
    {
        _handlerFactory = handlerFactory;
    }

    protected override async Task Handle(ConsumeContext<PasswordResetRequestMessage> context)
    {
        await _handlerFactory.Get<RequestPasswordResetHandler>().HandleAsync(context.Message.Email);
    }
}