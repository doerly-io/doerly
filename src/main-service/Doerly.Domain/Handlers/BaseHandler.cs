using Doerly.DataAccess;

namespace Doerly.Domain.Handlers;

public abstract class BaseHandler<TDbContext> : IHandler where TDbContext : IDbContext
{
    protected readonly TDbContext DbContext;

    public BaseHandler(TDbContext dbContext)
    {
        DbContext = dbContext;
    }
}

public abstract class BaseHandler : IHandler
{
    
}
