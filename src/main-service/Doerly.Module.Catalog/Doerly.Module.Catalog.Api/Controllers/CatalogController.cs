using Doerly.Api.Infrastructure;
using Doerly.Module.Catalog.Contracts.Dtos.Requests.Category;
using Doerly.Module.Catalog.Domain.Handlers.Category;
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
    [Route("api/[area]/[controller]")]
    public class CatalogController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest dto)
        {
            var result = await ResolveHandler<CreateCategoryHandler>().HandleAsync(dto);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await ResolveHandler<GetCategoryByIdHandler>().HandleAsync(id);
            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryRequest dto)
        {
            var result = await ResolveHandler<UpdateCategoryHandler>().HandleAsync(id, dto);
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
    }
}
