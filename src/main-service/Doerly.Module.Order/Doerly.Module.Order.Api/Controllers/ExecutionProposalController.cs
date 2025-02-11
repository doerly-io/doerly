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
    public async Task<IActionResult> SendExecutionProposal(SendExecutionProposalRequest dto)
    {
        var result = await ResolveHandler<SendExecutionProposalHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result);
    }

    [HttpPost("resolve")]
    public async Task<IActionResult> ResolveExecutionProposal(ResolveExecutionProposalRequest dto)
    {
        var result = await ResolveHandler<ResolveExecutionProposalHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExecutionProposalById(int id)
    {
        var result = await ResolveHandler<GetExecutionProposalByIdHandler>().HandleAsync(id);
        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateExecutionProposal(UpdateExecutionProposalRequest dto)
    {
        var result = await ResolveHandler<UpdateExecutionProposalHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("list")]
    public async Task<IActionResult> GetExecutionProposalsByPredicates(GetExecutionProposalsByPredicateRequest dto)
    {
        var result = await ResolveHandler<GetExecutionProposalsByPredicatesHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result);
    }

}
