using Doerly.Infrastructure.Api;
using Doerly.Module.Payments.Domain.Adapters;
using Doerly.Module.Payments.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Payments.Api.Controllers;

[Area("payments")]
[Route("api/[area]/[controller]")]
public class WebhookController : BaseApiController
{
    private readonly PaymentAdapterFactory _paymentAdapterFactory;

    public WebhookController(PaymentAdapterFactory paymentAdapterFactory)
    {
        _paymentAdapterFactory = paymentAdapterFactory;
    }

    [HttpPost("liqpay/final-status")]
    public async Task<IActionResult> FinalStatus([FromForm] string data, [FromForm] string signature)
    {
        var adapter = _paymentAdapterFactory(EPaymentAggregator.LiqPay);
        var result = await adapter.Adapt(data, signature);
        if (result.IsSuccess)
            return Ok();

        return BadRequest(result.ErrorMessage);
    }
}
