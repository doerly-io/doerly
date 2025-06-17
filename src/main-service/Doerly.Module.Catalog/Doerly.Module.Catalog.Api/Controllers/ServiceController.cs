using Doerly.Infrastructure.Api;
using Doerly.Module.Catalog.Contracts.Requests;
using Doerly.Module.Catalog.Domain.Handlers.Service;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Catalog.Api.Controllers
{
    [ApiController]
    [Area("service")]
    [Route("api/[controller]")]
    public class ServiceController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetService(int id)
        {
            var result = await ResolveHandler<GetServiceByIdHandler>().HandleAsync(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("paginated")]
        public async Task<IActionResult> GetServicesWithPagination(GetServiceWithPaginationRequest request)
        {
            var result = await ResolveHandler<GetServicesWithPaginationHandler>().HandleAsync(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpGet()]
        public async Task<IActionResult> GetServiceList()
        {
            var result = await ResolveHandler<GetServicesHandler>().HandleAsync();
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetServicesByUserId(int id)
        {
            var result = await ResolveHandler<GetServicesByUserIdHandler>().HandleAsync(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateService(CreateServiceRequest dto)
        {
            var result = await ResolveHandler<CreateServiceHandler>().HandleAsync(dto);

            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, UpdateServiceRequest dto)
        {
            var result = await ResolveHandler<UpdateServiceHandler>().HandleAsync(id, dto);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var result = await ResolveHandler<DeleteServiceHandler>().HandleAsync(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
