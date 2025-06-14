using Doerly.Domain;
using Doerly.Domain.Factories;
using Doerly.Messaging;
using Doerly.Module.Communication.DataTransferObjects.Messages;
using Doerly.Module.Notification.Domain.Handlers;
using Doerly.Module.Notification.Enums;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Notification.Messaging.Consumers;

public class NewMessageNotificationConsumer(
    ILogger<NewMessageNotificationConsumer> logger,
    IDoerlyRequestContext doerlyRequestContext,
    IHandlerFactory handlerFactory
    )
    : BaseConsumer<NewMessageNotificationMessage>(logger, doerlyRequestContext)
{
    protected override Task Handle(ConsumeContext<NewMessageNotificationMessage> context)
    {
        return handlerFactory.Get<SendNotificationHandler>().HandleAsync(
            context.Message.UserId,
            NotificationConstants.NewMessage,
            NotificationType.Message,
            context.Message.Data
        );
    }
} 