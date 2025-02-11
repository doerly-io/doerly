using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Domain.Models;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Domain.Dtos.Requests.ExecutionProposal;
using Doerly.Module.Order.Domain.Dtos.Responses.ExecutionProposal;
using Doerly.Module.Order.Localization;

using Microsoft.EntityFrameworkCore;

namespace Doerly.Module.Order.Domain.Handlers;
public class GetExecutionProposalsByPredicatesHandler : BaseOrderHandler
{
    public GetExecutionProposalsByPredicatesHandler(OrderDbContext context) : base(context)
    {}

    public async Task<HandlerResult<List<GetExecutionProposalResponse>>> HandleAsync(GetExecutionProposalsByPredicateRequest dto)
    {
        var executionProposals = DbContext.ExecutionProposals.AsNoTracking();
        if (dto.ReceiverId.HasValue)
        {
            executionProposals = executionProposals.Where(x => x.ReceiverId == dto.ReceiverId);
        }
        if (dto.SenderId.HasValue)
        {
            executionProposals = executionProposals.Where(x => x.SenderId == dto.SenderId);
        }

        var result = await executionProposals
            .Select(x => new GetExecutionProposalResponse
            {
                Id = x.Id,
                OrderId = x.OrderId,
                Comment = x.Comment,
                SenderId = x.SenderId,
                ReceiverId = x.ReceiverId,
                Status = x.Status
            })
            .ToListAsync();

        return HandlerResult.Success(result);
    }
}
