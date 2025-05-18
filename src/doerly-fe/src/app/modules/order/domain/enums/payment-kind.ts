export enum EPaymentKind {
    Post = 1,
    Pre = 2
}

export function getPaymentKindString(paymentKind: EPaymentKind): string {
    switch (paymentKind) {
        case EPaymentKind.Post:
            return 'Post';
        case EPaymentKind.Pre:
            return 'Pre';
        default:
            return '';
    }
}