using Doerly.Api.Infrastructure;
using Doerly.Domain.Models;
using Doerly.Module.Communication.Contracts.Dtos.Requests;
using Doerly.Module.Communication.Domain.Handlers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Communication.Api.Controllers;

[ApiController]
[Area("profile")]
[Route("api/[area]")]
public class CommunicationController : BaseApiController
{
    [HttpPost("messages/send")]
    [ProducesResponseType<HandlerResult<SendMessageDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateConversation(SendMessageDto requestDto)
    {
        var result = await ResolveHandler<SendMessageHandler>().HandleAsync(requestDto);
        
        if (!result.IsSuccess)
            return Conflict(result);

        return Ok();
    }
    
    [HttpGet("{messageId:int}")]
    [ProducesResponseType<HandlerResult<MessageDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult<MessageDto>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessage(int messageId)
    {
        var result = await ResolveHandler<GetMessageHandler>().HandleAsync(messageId);
        
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result.Value);
    }
}