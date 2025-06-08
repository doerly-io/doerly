using Doerly.Domain.Handlers;
using Doerly.Module.Communication.DataAccess;

namespace Doerly.Module.Communication.Domain.Handlers;

public class BaseCommunicationHandler(CommunicationDbContext dbContext) : BaseHandler<CommunicationDbContext>(dbContext);