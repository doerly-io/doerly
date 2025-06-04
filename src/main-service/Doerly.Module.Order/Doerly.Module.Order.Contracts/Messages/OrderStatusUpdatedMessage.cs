using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.Contracts.Messages;
public record OrderStatusUpdatedMessage(int OrderId, EOrderStatus OrderStatus);
