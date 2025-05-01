using Doerly.Api.Infrastructure;
using Doerly.Module.Payments.Api.Builders;
using Doerly.Module.Payments.Client.LiqPay;
using Doerly.Module.Payments.Contracts.Requests;
using Doerly.Module.Payments.Domain.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Payments.Api.Controllers;

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
        var invoiceCreateRequest = new CreateInvoiceRequest();
        var webhookUrl = _webhookUrlBuilder.BuildWebhookUrl(nameof(WebhookController) /*add ext metjod to get controller name*/, nameof(WebhookController.FinalStatus));
        var result = await ResolveHandler<CheckoutHandler>().HandlePaymentAsync(invoiceCreateRequest, webhookUrl);
        return Ok(result);
    }
}
