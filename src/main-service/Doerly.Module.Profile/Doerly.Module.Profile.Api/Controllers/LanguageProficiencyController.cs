using Doerly.Domain.Models;
using Doerly.Infrastructure.Api;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Module.Profile.Domain.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Profile.Api.Controllers;

[ApiController]
[Area("profile")]
[Route("api/[area]/{userId:int}/language-proficiency")]
public class LanguageProficiencyController : BaseApiController
{
    [HttpPost]
    [ProducesResponseType<OperationResult<int>>(StatusCodes.Status201Created)]
    [ProducesResponseType<OperationResult>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddLanguageProficiency(int userId, LanguageProficiencySaveDto dto)
    {
        var result = await ResolveHandler<CreateLanguageProficiencyHandler>().HandleAsync(userId, dto);
        if (!result.IsSuccess)
            return BadRequest(result);
            
        return Created();
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType<OperationResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateLanguageProficiency(int userId, int id, LanguageProficiencySaveDto dto)
    {
        var result = await ResolveHandler<UpdateLanguageProficiencyHandler>().HandleAsync(userId, id, dto);
        if (!result.IsSuccess)
            return NotFound(result);
            
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType<OperationResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteLanguageProficiency(int userId, int id)
    {
        var result = await ResolveHandler<DeleteLanguageProficiencyHandler>().HandleAsync(userId, id);
        if (!result.IsSuccess)
            return NotFound(result);
            
        return Ok(result);
    }
}