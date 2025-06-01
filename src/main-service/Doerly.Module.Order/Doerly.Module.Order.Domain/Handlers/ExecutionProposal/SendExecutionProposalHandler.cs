using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Enums;
using Doerly.Module.Order.DataAccess.Entities;
using Doerly.Module.Order.Contracts.Dtos;

using Microsoft.EntityFrameworkCore;
using Doerly.Localization;
using Doerly.Domain;
using Doerly.Messaging;

namespace Doerly.Module.Order.Domain.Handlers;
public class SendExecutionProposalHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;

    public SendExecutionProposalHandler(OrderDbContext dbContext, IDoerlyRequestContext doerlyRequestContext,
        IMessagePublisher messagePublisher) : base(dbContext, messagePublisher)
    {
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<HandlerResult<SendExecutionProposalResponse>> HandleAsync(SendExecutionProposalRequest dto)
    {
        var order = await DbContext.Orders.Select(x => new { x.Id, x.CustomerId }).FirstOrDefaultAsync(x => x.Id == dto.OrderId);
        if (order == null)
            return HandlerResult.Failure<SendExecutionProposalResponse>(Resources.Get("OrderNotFound"));

        /* firstly check if sender is a customer and then check if the receiver already got a proposal 
         * (simply checking receiver won't work because if receiver is a customer then there might be several proposals for him),
         * but if sender is executor (not a customer) then check if the sender already sent a proposal to the receiver
         */
        var existingExecutionProposal = await DbContext.ExecutionProposals
            .FirstOrDefaultAsync(x => x.OrderId == dto.OrderId && 
                ((x.SenderId == order.CustomerId && x.ReceiverId == dto.ReceiverId) ||
                (x.SenderId != order.CustomerId && x.SenderId == _doerlyRequestContext.UserId)));

        if (existingExecutionProposal != null)
            return HandlerResult.Failure<SendExecutionProposalResponse>(Resources.Get("ExecutionProposalAlreadySent"));

        var executionProposal = new ExecutionProposal()
        {
            OrderId = dto.OrderId,
            Comment = dto.Comment.Trim(),
            SenderId = _doerlyRequestContext.UserId ?? throw new Exception("We are fucked!"),
            ReceiverId = dto.ReceiverId,
            Status = EExecutionProposalStatus.Pending
        };

        DbContext.ExecutionProposals.Add(executionProposal);
        await DbContext.SaveChangesAsync();

        await PublishExecutionProposalStatusUpdatedEventAsync(executionProposal.Id, executionProposal.Status);

        var result = new SendExecutionProposalResponse()
        {
            Id = executionProposal.Id
        };

        return HandlerResult.Success(result);
    }
}
