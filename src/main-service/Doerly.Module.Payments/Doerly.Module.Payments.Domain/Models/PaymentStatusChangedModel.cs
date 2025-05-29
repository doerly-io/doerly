using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.Domain.Models;

public record PaymentStatusChangedModel(int PaymentId, EPaymentStatus Status);

