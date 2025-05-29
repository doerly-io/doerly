using Doerly.Domain.Models;
using Doerly.Localization;
using Doerly.Module.Order.DataAccess;
using Doerly.Module.Order.Contracts.Dtos;
using Doerly.Proxy.Profile;
using Doerly.Module.Order.DataAccess.Models;
using Doerly.Domain;

namespace Doerly.Module.Order.Domain.Handlers;
public class GetExecutionProposalByIdHandler : BaseOrderHandler
{
    private readonly IDoerlyRequestContext _doerlyRequestContext;
    private readonly IProfileModuleProxy _profileModuleProxy;
    public GetExecutionProposalByIdHandler(OrderDbContext dbContext, IProfileModuleProxy profileModuleProxy, IDoerlyRequestContext doerlyRequestContext) : base(dbContext)
    {
        _profileModuleProxy = profileModuleProxy;
        _doerlyRequestContext = doerlyRequestContext;
    }

    public async Task<HandlerResult<GetExecutionProposalResponse>> HandleAsync(int id)
    {
        var executionProposal = await DbContext.ExecutionProposals.FindAsync(id);

        if (executionProposal == null)
            return HandlerResult.Failure<GetExecutionProposalResponse>(Resources.Get("ExecutionProposalNotFound"));

        int profileId;
        if (_doerlyRequestContext.UserId == executionProposal.SenderId)
            profileId = executionProposal.ReceiverId;
        else if (_doerlyRequestContext.UserId == executionProposal.ReceiverId)
            profileId = executionProposal.SenderId;
        else
            throw new Exception(Resources.Get("InvalidAccess"));

        var customerProfile = await _profileModuleProxy.GetProfileAsync(profileId);

        var result = new GetExecutionProposalResponse
        {
            Id = executionProposal.Id,
            OrderId = executionProposal.OrderId,
            Comment = executionProposal.Comment,
            SenderId = executionProposal.SenderId,
            Sender = executionProposal.SenderId == customerProfile.Value.Id
                ? new ProfileInfo
                {
                    Id = customerProfile.Value.Id,
                    FirstName = customerProfile.Value.FirstName,
                    LastName = customerProfile.Value.LastName,
                    AvatarUrl = customerProfile.Value.ImageUrl
                }
                : null,
            ReceiverId = executionProposal.ReceiverId,
            Receiver = executionProposal.ReceiverId == customerProfile.Value.Id
                ? new ProfileInfo
                {
                    Id = customerProfile.Value.Id,
                    FirstName = customerProfile.Value.FirstName,
                    LastName = customerProfile.Value.LastName,
                    AvatarUrl = customerProfile.Value.ImageUrl
                }
                : null,
            Status = executionProposal.Status,
            DateCreated = executionProposal.DateCreated
        };

        return HandlerResult.Success(result);
    }
}
