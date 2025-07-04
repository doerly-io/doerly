﻿using Doerly.Infrastructure.Api;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.Domain.Handlers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Order.Api.Controllers;

[Authorize]
[ApiController]
[Area("order")]
[Route("api/[area]/[controller]")]
public class ExecutionProposalController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> SendExecutionProposal(SendExecutionProposalRequest dto)
    {
        var result = await ResolveHandler<SendExecutionProposalHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPut("resolve/{id}")]
    public async Task<IActionResult> ResolveExecutionProposal(int id, ResolveExecutionProposalRequest dto)
    {
        var result = await ResolveHandler<ResolveExecutionProposalHandler>().HandleAsync(id, dto);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetExecutionProposalById(int id)
    {
        var result = await ResolveHandler<GetExecutionProposalByIdHandler>().HandleAsync(id);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExecutionProposal(int id, UpdateExecutionProposalRequest dto)
    {
        var result = await ResolveHandler<UpdateExecutionProposalHandler>().HandleAsync(id, dto);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("list")]
    public async Task<IActionResult> GetExecutionProposalsByPredicates(GetExecutionProposalsWithPaginationRequest dto)
    {
        var result = await ResolveHandler<GetExecutionProposalsWithPaginationHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

}
