using Doerly.Api.Infrastructure;
using Doerly.Module.Order.Domain.Dtos.Requests.ExecutionProposal;
using Doerly.Module.Order.Domain.Handlers;

using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Order.Api.Controllers;

[ApiController]
[Area("execution-proposal")]
[Route("api/[area]")]
public class ExecutionProposalController : BaseApiController
{
    [HttpPost("send")]
    public async Task<IActionResult> Send(SendExecutionProposalRequest dto)
    {
        var result = await ResolveHandler<SendExecutionProposalHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result);
    }
}
