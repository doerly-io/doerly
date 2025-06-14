using Doerly.DataTransferObjects.Pagination;
using Doerly.Domain.Models;
using Doerly.Infrastructure.Api;
using Doerly.Localization;
using Doerly.Module.Communication.Api.Hubs;
using Doerly.Module.Communication.DataTransferObjects.Requests;
using Doerly.Module.Communication.DataTransferObjects.Responses;
using Doerly.Module.Communication.Domain.Handlers;
using Doerly.Module.Communication.Domain.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Doerly.Module.Communication.Api.Controllers;

[Authorize]
[ApiController]
[Area("communication")]
[Route("api/[area]")]
public class CommunicationController(IHubContext<CommunicationHub, ICommunicationHub> communicationHub) : BaseApiController
{
    [HttpGet("conversations")]
    [ProducesResponseType<OperationResult<ConversationResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult<ConversationResponse>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConversations([FromQuery] GetConversationsWithPaginationRequest dto)
    {
        var useId = RequestContext.UserId;
        if (!useId.HasValue || useId.Value == 0)
            return Unauthorized();

        var userId = useId.Value;

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
    [ProducesResponseType<OperationResult<ConversationResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult<ConversationResponse>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetConversationById(int conversationId)
    {
        var result = await ResolveHandler<GetConversationByIdHandler>().HandleAsync(conversationId);

        if (!result.IsSuccess)
            return NotFound(result);
        

        return Ok(result);
    }
    
    [HttpPost("conversations")]
    [ProducesResponseType<OperationResult<ConversationResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateConversation([FromBody] CreateConversationRequest request)
    {
        var userId = RequestContext.UserId;
        if (!userId.HasValue || userId.Value == 0)
            return Unauthorized();

        var initiatorId = userId.Value;
        var result = await ResolveHandler<CreateConversationHandler>().HandleAsync(request, initiatorId);

        if (!result.IsSuccess)
            return Conflict(result);

        return Ok(result);
    }
    
    [HttpPost("messages/send")]
    [ProducesResponseType<OperationResult<SendMessageRequest>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendMessage(SendMessageRequest request)
    {
        var useId = RequestContext.UserId;
        if (!useId.HasValue || useId.Value == 0)
            return Unauthorized();

        var userId = useId.Value;
        
        var result = await ResolveHandler<SendMessageHandler>().HandleAsync(userId, request);
        
        if (!result.IsSuccess)
            return Conflict(result);

        return Ok(result);
    }
    
    [HttpPost("conversations/{conversationId:int}/messages/file/send")]
    [ProducesResponseType<OperationResult<SendMessageRequest>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> SendFileMessage(int conversationId, [FromForm] IFormFile imageFile)
    {
        var useId = RequestContext.UserId;
        if (!useId.HasValue || useId.Value == 0)
            return Unauthorized();

        var userId = useId.Value;

        var fileBytes = GetFormFileBytes(imageFile);
        var result = await ResolveHandler<SendFileMessageHandler>().HandleAsync(conversationId, userId, fileBytes, imageFile.FileName);
        var message = (await ResolveHandler<GetMessageByIdHandler>().HandleAsync(result.Value)).Value;

        // Notify the communication hub about the new file message
        await communicationHub.Clients.Group(conversationId.ToString()).ReceiveMessage(message);

        return Ok(result);
    }
    
    [HttpGet("messages/{messageId:int}")]
    [ProducesResponseType<OperationResult<MessageResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<OperationResult<MessageResponse>>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessage(int messageId)
    {
        var result = await ResolveHandler<GetMessageByIdHandler>().HandleAsync(messageId);
        
        if (!result.IsSuccess)
            return NotFound(result);

        return Ok(result);
    }
    
    [HttpGet("conversations/with-user/{recipientId:int}")]
    [ProducesResponseType<OperationResult<int?>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConversationWithUser(int recipientId)
    {
        var userId = RequestContext.UserId;
        if (!userId.HasValue || userId.Value == 0)
            return Unauthorized();

        var initiatorId = userId.Value;

        if (initiatorId == recipientId)
        {
            return BadRequest(OperationResult.Failure<int?>(Resources.Get("Communication.CannotCheckConversationWithSelf")));
        }

        var result = await ResolveHandler<CheckConversationExistsHandler>().HandleAsync(initiatorId, recipientId);
        return Ok(result);
    }
}