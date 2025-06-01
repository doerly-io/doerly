using Doerly.Infrastructure.Api;
using Doerly.Module.Catalog.Contracts.Dtos.Requests.Category;
using Doerly.Module.Catalog.Contracts.Dtos.Requests.Filter;
using Doerly.Module.Catalog.Contracts.Dtos.Requests.Service;
using Doerly.Module.Catalog.Domain.Handlers.Category;
using Doerly.Module.Catalog.Domain.Handlers.Filter;
using Doerly.Module.Catalog.Domain.Handlers.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.Api.Controllers
{
    [ApiController]
    [Area("catalog")]
    [Route("api/[controller]")]
    public class CatalogController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await ResolveHandler<GetCategoryByIdHandler>().HandleAsync(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryList()
        {
            var result = await ResolveHandler<GetCategoriesHandler>().HandleAsync();
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest dto)
        {
            var result = await ResolveHandler<CreateCategoryHandler>().HandleAsync(dto);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryRequest dto)
        {
            var result = await ResolveHandler<UpdateCategoryHandler>().HandleAsync(dto);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await ResolveHandler<DeleteCategoryHandler>().HandleAsync(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

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

            return Ok(result);
        }

        [HttpPut("/service/{id}")]
        public async Task<IActionResult> UpdateService(UpdateServiceRequest dto)
        {
            var result = await ResolveHandler<UpdateServiceHandler>().HandleAsync(dto);
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
        public async Task<IActionResult> UpdateFilter(UpdateFilterRequest dto)
        {
            var result = await ResolveHandler<UpdateFilterHandler>().HandleAsync(dto);
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
