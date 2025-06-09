using Doerly.DataTransferObjects;
using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Infrastructure.Api;
using Doerly.Module.Payments.DataTransferObjects;
using Doerly.Module.Payments.DataTransferObjects.Responses;
using Doerly.Module.Payments.Domain.Handlers;


namespace Doerly.Module.Payments.Contracts;

public class PaymentModuleWrapper : IPaymentModuleWrapper
{
    private readonly IHandlerFactory _handlerFactory;
    private readonly WebhookUrlBuilder _webhookUrlBuilder;

    public PaymentModuleWrapper(IHandlerFactory handlerFactory, WebhookUrlBuilder webhookUrlBuilder)
    {
        _handlerFactory = handlerFactory;
        _webhookUrlBuilder = webhookUrlBuilder;
    }

    public async Task<OperationResult<BaseCheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest)
    {
        var webhookUrl = _webhookUrlBuilder.BuildWebhookUrl("/api/payments/Webhook/liqpay/final-status");
        var checkoutResponse = await _handlerFactory.Get<CheckoutHandler>().HandleAsync(checkoutRequest, webhookUrl);
        return checkoutResponse;
    }

    public async Task<CursorPaginationResponse<PaymentHistoryItemResponse>> GetUserPayments(int userId, CursorPaginationRequest request)
    {
        var result = await _handlerFactory.Get<SelectUserPaymentsHandler>().HandleAsync(userId, request);
        return result;
    }

    public Task<PaymentStatisticsDto> GetPaymentStatisticsAsync()
    {
        return _handlerFactory.Get<GetPaymentStatisticsHandler>().HandleAsync();
    }
}
