using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.BaseClient;

public delegate IBasePaymentClient PaymentClientFactory(EPaymentAggregator paymentAggregator);
