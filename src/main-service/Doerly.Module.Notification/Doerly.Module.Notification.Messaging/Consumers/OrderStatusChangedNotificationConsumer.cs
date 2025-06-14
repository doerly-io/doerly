using System.Text.Json;
using Doerly.Domain;
using Doerly.Domain.Factories;
using Doerly.Messaging;
using Doerly.Module.Notification.Domain.Handlers;
using Doerly.Module.Notification.Enums;
using Doerly.Module.Order.DataTransferObjects.Messages;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Notification.Messaging.Consumers;

public class OrderStatusChangedNotificationConsumer(
    ILogger<OrderStatusChangedNotificationConsumer> logger,
    IDoerlyRequestContext doerlyRequestContext,
    IHandlerFactory handlerFactory,
    IDoerlyRequestContext requestContext)
    : BaseConsumer<OrderStatusUpdatedMessage>(logger, doerlyRequestContext)
{
    protected override Task Handle(ConsumeContext<OrderStatusUpdatedMessage> context)
    {
        return handlerFactory.Get<SendNotificationHandler>().HandleAsync(
            (int)requestContext.UserId,
            NotificationConstants.StatusChanged,
            NotificationType.Order,
            JsonSerializer.Serialize(new { orderId = context.Message.OrderId, orderStatus = context.Message.OrderStatus })
        );
    }
} 