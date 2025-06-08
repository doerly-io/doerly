using Doerly.DataTransferObjects.Pagination;
using Doerly.Infrastructure.Api;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Module.Order.Domain.Handlers.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Catalog.Api.Controllers;

[Authorize]
[ApiController]
[Area("catalog")]
[Route("api/[area]/[controller]")]
public class OrdersController : BaseApiController
{
    [HttpGet("get-orders-with-pagination")]
    [ProducesResponseType<BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrdersWithPagination([FromQuery] GetOrderWithFilterAndPaginationRequest request)
    {
        var result = await ResolveHandler<SelectOrdersWithFilterAndPaginationHandler>().HandleAsync(request);
        return Ok(result);
    }
}
