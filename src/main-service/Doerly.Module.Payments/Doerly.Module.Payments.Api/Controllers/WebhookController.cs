using Doerly.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Payments.Api.Controllers;

[Area("payments")]
[Route("api/[area]/[controller]")]
public class WebhookController : BaseApiController
{
    [HttpPost("liqpay/final-status")]
    public async Task<IActionResult> FinalStatus([FromQuery] string data, [FromQuery] string signature)
    {
        var result = await ResolveHandler<LiqPayFinalStatus>().HandleAsync(data, signature);

    } 
    
}
