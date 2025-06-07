using Doerly.DataTransferObjects;
using Doerly.Domain.Factories;
using Doerly.Domain.Models;
using Doerly.Extensions;
using Doerly.Infrastructure.Api;
using Doerly.Module.Payments.Api.Controllers;
using Doerly.Module.Payments.Contracts;
using Doerly.Module.Payments.Domain.Handlers;

namespace Doerly.Module.Payments.Api.ModuleWrapper;

public interface IPaymentModuleWrapper
{
    Task<HandlerResult<BaseCheckoutResponse>> CheckoutAsync(CheckoutRequest checkoutRequest);

    Task<CursorPaginationResponse<PaymentHistoryItemResponse>> GetUserPayments(int userId, CursorPaginationRequest request);
}

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
}
