export enum EOrderStatus { 
    Placed = 1,
    InProgress = 2,
    AwaitingPayment = 3,
    AwaitingConfirmation = 4,
    Completed = 5,
    Canceled = 6,
}

export function getOrderStatusSeverity(status: EOrderStatus): "success" | "secondary" | "info" | "warn" | "danger" | "contrast" | undefined {
    switch (status) {
        case EOrderStatus.Placed:
            return 'info';
        case EOrderStatus.InProgress:
            return 'warn';
        case EOrderStatus.Completed:
            return 'success';
        case EOrderStatus.Canceled:
            return 'danger';
        default:
            return 'secondary';
    }
}