using System.Security.Claims;
using Doerly.Api.Infrastructure;
using Doerly.Domain.Models;
using Doerly.Module.Communication.Contracts.Dtos.Requests;
using Doerly.Module.Communication.Contracts.Dtos.Responses;
using Doerly.Module.Communication.Domain.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doerly.Module.Communication.Api.Controllers;

[Authorize]
[ApiController]
[Area("communication")]
[Route("api/[area]")]
public class CommunicationController : BaseApiController
{
    [HttpGet("conversations/{conversationId:int}")]
    [ProducesResponseType<HandlerResult<ConversationResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult<ConversationResponseDto>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConversation(int conversationId)
    {
        var result = await ResolveHandler<GetConversationHandler>().HandleAsync(conversationId);

        if (!result.IsSuccess)
            return NotFound(result);
        

        return Ok(result.Value);
    }
    
    [HttpPost("messages/send")]
    [ProducesResponseType<HandlerResult<SendMessageRequestDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendMessage(SendMessageRequestDto requestRequestDto)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userId = int.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);
        if(userId == 0)
            return Unauthorized();
        
        requestRequestDto.InitiatorId = userId;
        var result = await ResolveHandler<SendMessageHandler>().HandleAsync(requestRequestDto);
        
        if (!result.IsSuccess)
            return Conflict(result);

        return Ok();
    }
    
    [HttpGet("messages/{messageId:int}")]
    [ProducesResponseType<HandlerResult<MessageResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult<MessageResponseDto>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessage(int messageId)
    {
        var result = await ResolveHandler<GetMessageHandler>().HandleAsync(messageId);
        
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result.Value);
    }
}