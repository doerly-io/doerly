namespace Doerly.Module.Order.Enums;

public enum EOrderStatus : byte
{
    Placed = 1,
    InProgress = 2,
    AwaitingPayment = 3,
    AwaitingConfirmation = 4,
    Completed = 5,
    Canceled = 6
}
