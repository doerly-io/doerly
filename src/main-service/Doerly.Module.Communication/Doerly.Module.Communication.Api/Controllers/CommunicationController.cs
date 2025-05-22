using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Models;
using Doerly.Infrastructure.Api;
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
    [HttpGet("conversations")]
    [ProducesResponseType<HandlerResult<ConversationResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult<ConversationResponseDto>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConversations([FromQuery] GetConversationsWithPaginationRequest dto)
    {
        var userId = GetUserId();
        if(userId == 0)
            return Unauthorized();

        var pagination = new GetEntitiesWithPaginationRequest()
        {
            PageInfo = new PageInfo()
            {
                Number = dto.PageNumber,
                Size = dto.PageSize
            }
        };
        var result = await ResolveHandler<GetUserConversationsWithPaginationHandler>().HandleAsync(userId, pagination);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
    
    [HttpGet("conversations/{conversationId:int}")]
    [ProducesResponseType<HandlerResult<ConversationResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult<ConversationResponseDto>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConversationById(int conversationId)
    {
        var result = await ResolveHandler<GetConversationByIdHandler>().HandleAsync(conversationId);

        if (!result.IsSuccess)
            return NotFound(result);
        

        return Ok(result);
    }
    
    [HttpPost("messages/send")]
    [ProducesResponseType<HandlerResult<SendMessageRequest>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendMessage(SendMessageRequest requestRequest)
    {
        var userId = GetUserId();
        requestRequest.InitiatorId = userId;
        var result = await ResolveHandler<SendMessageHandler>().HandleAsync(requestRequest);
        
        if (!result.IsSuccess)
            return Conflict(result);

        return Ok();
    }
    
    [HttpGet("messages/{messageId:int}")]
    [ProducesResponseType<HandlerResult<MessageResponseDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<HandlerResult<MessageResponseDto>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessage(int messageId)
    {
        var result = await ResolveHandler<GetMessageByIdHandler>().HandleAsync(messageId);
        
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
}