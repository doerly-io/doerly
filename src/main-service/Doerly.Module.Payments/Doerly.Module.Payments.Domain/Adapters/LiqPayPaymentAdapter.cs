using System.Text;
using System.Text.Json;
using Doerly.Domain.Factories;
using Doerly.Domain.Helpers;
using Doerly.Domain.Models;
using Doerly.Module.Payments.BaseClient;
using Doerly.Module.Payments.Client.LiqPay;
using Doerly.Module.Payments.Contracts.Messages;
using Doerly.Module.Payments.Domain.Handlers;
using Doerly.Module.Payments.Domain.Models;
using Doerly.Module.Payments.Enums;
using Microsoft.Extensions.Logging;

namespace Doerly.Module.Payments.Domain.Adapters;

public class LiqPayPaymentAdapter : IPaymentAdapter
{
    private readonly IHandlerFactory _handlerFactory;
    private readonly PaymentClientFactory _paymentClientFactory;
    private readonly ILogger<LiqPayPaymentAdapter> _logger;

    public LiqPayPaymentAdapter(
        IHandlerFactory handlerFactory,
        PaymentClientFactory paymentClientFactory,
        ILogger<LiqPayPaymentAdapter> logger
    )
    {
        _handlerFactory = handlerFactory;
        _paymentClientFactory = paymentClientFactory;
        _logger = logger;
    }

    public async Task<HandlerResult> Adapt(string data, string? signature = null)
    {
        var client = _paymentClientFactory(EPaymentAggregator.LiqPay);
        var isValidSignature = client.ValidateSignature(data, signature!);
        if (!isValidSignature)
        {
            _logger.LogWarning("Failed to parse LiqPay checkout response or validate signature. Data: {data}, Signature: {signature}", data, signature);
            return HandlerResult.Failure("Failed to validate signature");
        }

        var decodedData = Convert.FromBase64String(data);
        var jsonData = Encoding.UTF8.GetString(decodedData);
        var liqPayCheckoutStatusResult = SafeExecutor.Execute(() => JsonSerializer.Deserialize<LiqPayCheckoutStatusChangedDto>(jsonData));

        if (!liqPayCheckoutStatusResult.IsSuccess)
        {
            _logger.LogWarning("Failed to deserialize LiqPay checkout response. Data: {data}, Signature: {signature}", data, signature);
            return HandlerResult.Failure("Failed to parse checkout response");
        }
        
        var liqPayCheckoutStatus = liqPayCheckoutStatusResult.Value;
        if (!int.TryParse(liqPayCheckoutStatus.OrderId, out var paymentId))
        {
            _logger.LogWarning("Invalid order Id in LiqPay checkout response. PaymentId: {PaymentId}", liqPayCheckoutStatus.OrderId);
            return HandlerResult.Failure("Invalid order Id");
        }

        var paymentStatusChangedHandler = _handlerFactory.Get<PaymentStatusChangedHandler>();
        var result = await paymentStatusChangedHandler.Handle(new PaymentStatusChangedModel(paymentId, EPaymentStatus.Completed));
        return result;
    }
}
