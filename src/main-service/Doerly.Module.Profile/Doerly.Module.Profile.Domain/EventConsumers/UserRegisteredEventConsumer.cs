using Doerly.Domain.Factories;
using Doerly.Module.Authorization.Contracts.Messages;
using Doerly.Module.Profile.Contracts.Dtos;
using Doerly.Module.Profile.Domain.Handlers;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Profile.Domain.EventConsumers;

public class UserRegisteredEventConsumer : IConsumer<UserRegisteredMessage>
{
    private readonly ILogger<UserRegisteredEventConsumer> _logger;
    private readonly IHandlerFactory _handlerFactory;

    public UserRegisteredEventConsumer(ILogger<UserRegisteredEventConsumer> logger, IHandlerFactory handlerFactory)
    {
        _logger = logger;
        _handlerFactory = handlerFactory;
    }

    public async Task Consume(ConsumeContext<UserRegisteredMessage> context)
    {
        var profileSaveDto = new ProfileSaveDto
        {
            UserId = context.Message.UserId,
            FirstName = context.Message.FirstName,
            LastName = context.Message.LastName,
        };

        await _handlerFactory.Get<CreateProfileHandler>().HandleAsync(profileSaveDto);
    }
}
