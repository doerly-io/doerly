using Doerly.DataTransferObjects.Pagination;
using Doerly.Infrastructure.Api;
using Doerly.Module.Catalog.Domain.Handlers.Orders;
using Doerly.Module.Order.DataTransferObjects.Requests;
using Doerly.Module.Order.DataTransferObjects;
using Doerly.Module.Order.DataTransferObjects.Responses;
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
    [AllowAnonymous]
    [HttpGet("get-orders-with-pagination")]
    [ProducesResponseType<BasePaginationResponse<GetOrdersWithPaginationAndFilterResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrdersWithPagination([FromQuery] GetOrderWithFilterAndPaginationRequest request)
    {
        var result = await ResolveHandler<SelectOrdersWithFilterAndPaginationHandler>().HandleAsync(request);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("get-orders-amount-by-categories")]
    [ProducesResponseType<List<GetOrdersAmountByCategoriesResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrdersAmountByCategories()
    {
        var result = await ResolveHandler<GetOrdersAmountByCategoriesHandler>().HandleAsync();
        return Ok(result);
    }
}
