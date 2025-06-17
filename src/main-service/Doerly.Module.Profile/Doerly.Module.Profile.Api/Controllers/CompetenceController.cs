using Doerly.Domain.Models;
using Doerly.Infrastructure.Api;
using Doerly.Module.Profile.DataTransferObjects;
using Doerly.Module.Profile.Domain.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Profile.Api.Controllers;

[ApiController]
[Area("profile")]
[Route("api/[area]/{userId:int}/[controller]")]
public class CompetenceController : BaseApiController
{
    [HttpPost]
    [ProducesResponseType<OperationResult<int>>(StatusCodes.Status201Created)]
    [ProducesResponseType<OperationResult>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddCompetence(int userId, CompetenceSaveDto dto)
    {
        var result = await ResolveHandler<CreateCompetenceHandler>().HandleAsync(userId, dto);
        if (!result.IsSuccess)
            return BadRequest(result);
            
        return Created();
    }
    
    [HttpDelete("{competenceId:int}")]
    [ProducesResponseType<OperationResult>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteCompetence(int userId, int competenceId)
    {
        var result = await ResolveHandler<DeleteCompetenceHandler>().HandleAsync(userId, competenceId);
        if (!result.IsSuccess)
            return BadRequest(result);
        
        return Ok(result);
    }
}