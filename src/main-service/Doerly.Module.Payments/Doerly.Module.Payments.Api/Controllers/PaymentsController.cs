using Doerly.DataTransferObjects;
using Doerly.Infrastructure.Api;
using Doerly.Extensions;
using Doerly.Module.Payments.Contracts;
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

    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        var invoiceCreateRequest = new CheckoutRequest
        {
            PayerId = 1,
            AmountTotal = 150.25M,
            BillDescription = "Hello world",
            PaymentDescription = "Hello world",
            ReturnUrl = null,
            Currency = ECurrency.UAH,
            PaymentAction = EPaymentAction.Pay
        };

        var uri = _webhookUrlBuilder.BuildWebhookUrl(nameof(WebhookController).ToControllerName(), nameof(WebhookController.FinalStatus));
        var result = await ResolveHandler<CheckoutHandler>().HandleAsync(invoiceCreateRequest, uri);
        return Ok(result);
    }

    
    [HttpGet("user")]
    public async Task<IActionResult> GetUserPayments([FromQuery] CursorPaginationRequest request)
    {
        var result = await ResolveHandler<SelectUserPaymentsHandler>().HandleAsync(request);
        return Ok(result);
    }
}
