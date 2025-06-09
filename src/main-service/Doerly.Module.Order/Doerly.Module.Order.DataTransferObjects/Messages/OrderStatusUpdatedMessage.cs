using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.DataTransferObjects.Messages;
public record OrderStatusUpdatedMessage(int OrderId, EOrderStatus OrderStatus);
