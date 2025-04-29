using Doerly.Api.Infrastructure;
using Doerly.Module.Payments.Domain.WebhookHandlers;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Payments.Api.Controllers;

[Area("payments")]
[Route("api/[area]/[controller]")]
public class WebhookController : BaseApiController
{
    [HttpPost("liqpay/final-status")]
    public async Task<IActionResult> FinalStatus([FromQuery] string data, [FromQuery] string signature)
    {
        await ResolveHandler<LiqPayFinalStatusHandler>().HandleAsync(data, signature);

        return Ok();
    }
}
