using Doerly.DataTransferObjects;
using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Extensions;
using Doerly.Infrastructure.Api;
using Doerly.Module.Payments.Api.Controllers;
using Doerly.Module.Payments.Contracts;
using Doerly.Module.Payments.Contracts.Responses;
using Doerly.Module.Payments.Domain.Handlers;
using Doerly.Module.Payments.Domain.Handlers.Metrics;
using Doerly.Proxy.Payment;

namespace Doerly.Module.Payments.Api.ModuleWrapper;

public class PaymentModuleWrapper : IPaymentModuleWrapper
{
    private readonly IHandlerFactory _handlerFactory;
    private readonly WebhookUrlBuilder _webhookUrlBuilder;

    public PaymentModuleWrapper(IHandlerFactory handlerFactory, WebhookUrlBuilder webhookUrlBuilder)
    {
        _handlerFactory = handlerFactory;
        _webhookUrlBuilder = webhookUrlBuilder;
    }

    public async Task<HandlerResult<BaseCheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest)
    {
        var webhookUrl =
            _webhookUrlBuilder.BuildWebhookUrl(nameof(WebhookController).ToControllerName(), nameof(WebhookController.FinalStatus));
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
