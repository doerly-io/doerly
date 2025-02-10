using Doerly.Api.Infrastructure;
using Doerly.Domain.Models;
using Doerly.Module.Order.Domain.Dtos.Requests.Order;
using Doerly.Module.Order.Domain.Handlers;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Order.Api.Controllers;

[ApiController]
[Area("order")]
[Route("api/[area]")]
public class OrderController : BaseApiController
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest dto)
    {
        var result = await ResolveHandler<CreateOrderHandler>().HandleAsync(dto);

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> CreateOrder(int id)
    {
        var result = await ResolveHandler<GetOrderByIdHandler>().HandleAsync(id);
        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetOrderHistory([FromQuery] GetOrderHistoryRequest dto)
    {
        var result = await ResolveHandler<GetOrderHistoryHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result);
    }
}
