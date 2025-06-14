using Doerly.Infrastructure.Api;
using Doerly.Module.Catalog.Contracts.Requests;
using Doerly.Module.Catalog.Domain.Handlers.Category;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Catalog.Api.Controllers;

[ApiController]
[Area("category")]
[Route("api/[controller]")]
public class CategoryController : BaseApiController
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

        return Created();
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
