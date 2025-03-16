export enum PaymentKind {
    Post,
    Pre
}

export function getPaymentKindString(paymentKind: PaymentKind): string {
    switch (paymentKind) {
        case PaymentKind.Post:
            return 'Post';
        case PaymentKind.Pre:
            return 'Pre';
        default:
            return '';
    }
}