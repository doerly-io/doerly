using Doerly.Api.Infrastructure;
using Doerly.Module.Payments.Domain.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Payments.Api.Controllers;

[ApiController]
[Area("payments")]
[Route("api/[area]/[controller]")]
public class PaymentsController : BaseApiController
{
    // [HttpGet("test")]
    // public async Task<IActionResult> Test()
    // {
    //     var result = await ResolveHandler<CheckoutHandler>().HandlePaymentAsync();
    //     return Ok(result);
    // }
}
