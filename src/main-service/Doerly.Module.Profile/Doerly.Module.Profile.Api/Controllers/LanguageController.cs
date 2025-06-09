using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Models;
using Doerly.Infrastructure.Api;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Module.Profile.Domain.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Profile.Api.Controllers;

[ApiController]
[Area("profile")]
[Route("api/[area]/languages")]
public class LanguageController : BaseApiController
{
    [HttpPost("list")]
    [ProducesResponseType<OperationResult<PageDto<LanguageDto>>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLanguages(LanguagesQueryDto dto)
    {
        var result = await ResolveHandler<GetLanguagesHandler>().HandleAsync(dto);
        return Ok(result);
    }
    
    [HttpPost]
    [ProducesResponseType<OperationResult<int>>(StatusCodes.Status201Created)]
    [ProducesResponseType<OperationResult>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateLanguage(LanguageSaveDto dto)
    {
        var result = await ResolveHandler<CreateLanguageHandler>().HandleAsync(dto);
        if (!result.IsSuccess)
            return BadRequest(result);
            
        return Created();
    }
    
    
    [HttpPut("{id:int}")]
    [ProducesResponseType<OperationResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult>(StatusCodes.Status404NotFound)] 
    public async Task<IActionResult> UpdateLanguage(int id, LanguageSaveDto dto)
    {
        var result = await ResolveHandler<UpdateLanguageHandler>().HandleAsync(id, dto);
        if (!result.IsSuccess)
            return NotFound(result);
            
        return Ok(result);
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType<OperationResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLanguage(int id)
    {
        var result = await ResolveHandler<DeleteLanguageHandler>().HandleAsync(id);
        if (!result.IsSuccess)
            return NotFound(result);
            
        return Ok(result);
    }
}