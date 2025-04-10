using Doerly.Domain.Handlers;
using Doerly.Module.Communication.DataAccess;

namespace Doerly.Module.Communication.Domain.Handlers;

public class BaseCommunicationHandler : BaseHandler<CommunicationDbContext>
{
    public BaseCommunicationHandler(CommunicationDbContext dbContext) : base(dbContext)
    {
    }
}