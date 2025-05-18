using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.Contracts.Messages;

public record PaymentStatusChangedMessage(int BillId, EPaymentStatus Status);
