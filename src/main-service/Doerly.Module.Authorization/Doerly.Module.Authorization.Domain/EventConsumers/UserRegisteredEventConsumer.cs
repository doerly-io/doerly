using Doerly.Domain.Factories;
using Doerly.Messaging;
using Doerly.Module.Authorization.Domain.Handlers;
using Doerly.Module.Authorization.Contracts.Messages;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Authorization.Domain.EventConsumers;

public class UserRegisteredEventConsumer : BaseConsumer<UserRegisteredMessage>
{
    private readonly IHandlerFactory _handlerFactory;
    private readonly IHostEnvironment _hostEnvironment;

    public UserRegisteredEventConsumer(
        ILogger<UserRegisteredEventConsumer> logger,
        IHandlerFactory handlerFactory,
        IHostEnvironment hostEnvironment
        ) : base(logger)
    {
        _handlerFactory = handlerFactory;
        _hostEnvironment = hostEnvironment;
    }

    protected override async Task Handle(ConsumeContext<UserRegisteredMessage> context)
    {
        // do not send email in development environment due to cost saving reasons
        if (_hostEnvironment.IsDevelopment())
            return;

        await _handlerFactory.Get<RequestEmailVerificationHandler>().HandleAsync(context.Message);
    }
}
