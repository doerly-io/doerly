using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.Domain.Models;

public record PaymentStatusChangedModel(Guid PaymentGuid, EPaymentStatus Status, EPaymentMethod PaymentMethod, string? CardNumber);

