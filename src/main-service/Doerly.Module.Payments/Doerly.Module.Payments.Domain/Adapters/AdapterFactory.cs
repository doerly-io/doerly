using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.Domain.Adapters;

public delegate IPaymentAdapter PaymentAdapterFactory(EPaymentAggregator paymentAggregator);


