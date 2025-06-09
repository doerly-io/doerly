using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.DataTransferObjects.Messages;

public record BillStatusChangedMessage(int BillId, Guid PaymentGuid, EPaymentStatus Status);
