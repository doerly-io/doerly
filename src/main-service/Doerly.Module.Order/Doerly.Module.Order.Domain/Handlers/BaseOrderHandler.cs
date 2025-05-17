using Doerly.Domain.Handlers;
using Doerly.Module.Order.DataAccess;
using Microsoft.Extensions.Options;

namespace Doerly.Module.Order.Domain.Handlers;

public class BaseOrderHandler : BaseHandler<OrderDbContext>
{
    public BaseOrderHandler(OrderDbContext dbContext) : base(dbContext)
    {}
}
