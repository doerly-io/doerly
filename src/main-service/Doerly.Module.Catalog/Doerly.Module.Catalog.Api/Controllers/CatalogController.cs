using Doerly.Infrastructure.Api;
using Doerly.Module.Catalog.Contracts.Requests;
using Doerly.Module.Catalog.Domain.Handlers.Filter;
using Doerly.Module.Catalog.Domain.Handlers.Service;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Catalog.Api.Controllers
{
    [ApiController]
    [Area("catalog")]
    [Route("api/[controller]")]
    public class CatalogController : BaseApiController
    {
        [HttpGet("/filter/{id}")]
        public async Task<IActionResult> GetFiltersByCategoryId(int id)
        {
            var result = await ResolveHandler<GetFiltersByCategoryIdHandler>().HandleAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("/filter")]
        public async Task<IActionResult> CreateFilter(CreateFilterRequest dto)
        {
            var result = await ResolveHandler<CreateFilterHandler>().HandleAsync(dto);
            return Ok(result);
        }

        [HttpPut("/filter/{id}")]
        public async Task<IActionResult> UpdateFilter(int id, UpdateFilterRequest dto)
        {
            var result = await ResolveHandler<UpdateFilterHandler>().HandleAsync(id, dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("/filter/{id}")]
        public async Task<IActionResult> DeleteFilter(int id)
        {
            var result = await ResolveHandler<DeleteFilterHandler>().HandleAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
