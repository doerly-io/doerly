using Doerly.Domain.Factories;
using Doerly.Module.Authorization.Domain.Handlers;
using Doerly.Module.Authorization.Contracts.Messages;
using MassTransit;

namespace Doerly.Module.Authorization.Domain.EventConsumers;

public class UserRegisteredEventConsumer : IConsumer<UserRegisteredMessage>
{
    private readonly IHandlerFactory _handlerFactory;

    public UserRegisteredEventConsumer(IHandlerFactory handlerFactory)
    {
        _handlerFactory = handlerFactory;
    }

    public async Task Consume(ConsumeContext<UserRegisteredMessage> context)
    {
        // do not send email in development environment due to cost saving reasons
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            return;

        await _handlerFactory.Get<RequestEmailVerificationHandler>().HandleAsync(context.Message);
    }
}
