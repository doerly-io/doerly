using Doerly.Domain;
using Doerly.Domain.Factories;
using Doerly.Messaging;
using Doerly.Module.Authorization.DataTransferObjects.Messages;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Module.Profile.Domain.Handlers;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Profile.Domain.EventConsumers;

public class UserRegisteredEventConsumer : BaseConsumer<UserRegisteredMessage>
{
    private readonly IHandlerFactory _handlerFactory;

    public UserRegisteredEventConsumer(
        ILogger<UserRegisteredEventConsumer> logger,
        IDoerlyRequestContext doerlyRequestContext,
        IHandlerFactory handlerFactory) : base(logger, doerlyRequestContext)
    {
        _handlerFactory = handlerFactory;
    }

    protected override async Task Handle(ConsumeContext<UserRegisteredMessage> context)
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