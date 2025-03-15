using Doerly.Api.Infrastructure;
using Doerly.Domain.Models;
using Doerly.Module.Order.Domain.Dtos.Requests;
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

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrdersWithPagination([FromQuery] GetOrdersWithPaginationRequest dto)
    {
        var result = await ResolveHandler<GetOrdersWithPaginationHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    /*[HttpGet("orders")]
    public async Task<IActionResult> GetOrderHistory([FromQuery] GetItemsWithPaginationByPredicatesRequest dto)
    {
        var result = await ResolveHandler<GetOrdersHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }*/

    [HttpPut("update")]
    public async Task<IActionResult> UpdateOrder(UpdateOrderRequest dto)
    {
        var result = await ResolveHandler<UpdateOrderHandler>().HandleAsync(dto);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var result = await ResolveHandler<DeleteOrderHandler>().HandleAsync(id);
        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result);
    }
}
