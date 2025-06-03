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
        [HttpGet("/service/{id}")]
        public async Task<IActionResult> GetService(int id)
        {
            var result = await ResolveHandler<GetServiceByIdHandler>().HandleAsync(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("/service/paginated")]
        public async Task<IActionResult> GetServicesWithPagination(GetServiceWithPaginationRequest request)
        {
            var result = await ResolveHandler<GetServicesWithPaginationHandler>().HandleAsync(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpGet("/service")]
        public async Task<IActionResult> GetServiceList()
        {
            var result = await ResolveHandler<GetServicesHandler>().HandleAsync();
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("/service")]
        public async Task<IActionResult> CreateService(CreateServiceRequest dto)
        {
            var result = await ResolveHandler<CreateServiceHandler>().HandleAsync(dto);

            return Created();
        }

        [HttpPut("/service/{id}")]
        public async Task<IActionResult> UpdateService(int id, UpdateServiceRequest dto)
        {
            var result = await ResolveHandler<UpdateServiceHandler>().HandleAsync(id, dto);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("/service/{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var result = await ResolveHandler<DeleteServiceHandler>().HandleAsync(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

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
