using Doerly.DataTransferObjects;
using Doerly.Infrastructure.Api;
using Doerly.Module.Payments.DataTransferObjects;
using Doerly.Module.Payments.Domain.Handlers;
using Doerly.Module.Payments.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Payments.Api.Controllers;

[Authorize]
[ApiController]
[Area("payments")]
[Route("api/[area]/[controller]")]
public class PaymentsController : BaseApiController
{
    private readonly WebhookUrlBuilder _webhookUrlBuilder;

    public PaymentsController(WebhookUrlBuilder webhookUrlBuilder)
    {
        _webhookUrlBuilder = webhookUrlBuilder;
    }

    //[AllowAnonymous]
    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        var invoiceCreateRequest = new CheckoutRequest
        {
            PayerId = 1,
            AmountTotal = 150.25M,
            BillDescription = "Hello world",
            PaymentDescription = "Тестова оплата",
            ReturnUrl = null,
            Currency = ECurrency.UAH,
            PaymentAction = EPaymentAction.Pay,
            PayerEmail = RequestContext.UserEmail
        };

        var uri = _webhookUrlBuilder.BuildWebhookUrl("/api/payments/Webhook/liqpay/update-status");
        var result = await ResolveHandler<CheckoutHandler>().HandleAsync(invoiceCreateRequest, uri);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("payments-history")]
    public async Task<IActionResult> GetUserPayments([FromQuery] CursorPaginationRequest request)
    {
        var userId = RequestContext.UserId;
        var result = await ResolveHandler<SelectUserPaymentsHandler>().HandleAsync(userId.Value, request);
        return Ok(result);
    }
}
