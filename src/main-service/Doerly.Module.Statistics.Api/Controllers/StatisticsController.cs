using Doerly.Domain.Models;
using Doerly.Infrastructure.Api;
using Doerly.Module.Statistics.Contracts.Dtos;
using Doerly.Module.Statistics.Domain.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Statistics.Api.Controllers;

[ApiController]
[Area("statistics")]
[Route("api/[area]")]
public class StatisticsController : BaseApiController
{
    [HttpGet("activity-users")]
    [ProducesResponseType<HandlerResult<UserActivityStatisticsDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActivityUsersStatistics()
    {
        var result = await ResolveHandler<GetActivityUsersStatisticsHandler>().HandleAsync();
        return Ok(result);
    }
    
    [HttpGet("payment")]
    [ProducesResponseType<HandlerResult<PaymentStatisticsDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentStatistics()
    {
        var result = await ResolveHandler<GetPaymentStatisticsHandler>().HandleAsync();
        return Ok(result);
    }
}