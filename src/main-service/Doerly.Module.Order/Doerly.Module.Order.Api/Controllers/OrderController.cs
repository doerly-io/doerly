using Doerly.Infrastructure.Api;
using Doerly.Module.Order.Domain.Dtos.Requests;
using Doerly.Module.Order.Domain.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Order.Api.Controllers;

[ApiController]
[Area("order")]
[Route("api/[area]/[controller]")]
public class OrderController : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest dto)
    {
        var result = await ResolveHandler<CreateOrderHandler>().HandleAsync(dto);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var result = await ResolveHandler<GetOrderByIdHandler>().HandleAsync(id);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("list")]
    public async Task<IActionResult> GetOrdersWithPagination(GetOrdersWithPaginationRequest dto)
    {
        var result = await ResolveHandler<GetOrdersWithPaginationHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, UpdateOrderRequest dto)
    {
        var result = await ResolveHandler<UpdateOrderHandler>().HandleAsync(id, dto);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        var result = await ResolveHandler<CancelOrderHandler>().HandleAsync(id);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }
}
