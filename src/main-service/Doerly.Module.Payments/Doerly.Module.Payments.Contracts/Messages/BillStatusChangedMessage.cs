using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.Contracts.Messages;

public record BillStatusChangedMessage(int BillId, EPaymentStatus Status);
