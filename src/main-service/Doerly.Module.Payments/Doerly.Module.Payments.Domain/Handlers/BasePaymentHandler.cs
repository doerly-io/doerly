using Doerly.Domain.Handlers;
using Doerly.Module.Payments.DataAccess;

namespace Doerly.Module.Payments.Domain.Handlers;

public class BasePaymentHandler : BaseHandler<PaymentDbContext>
{
    public BasePaymentHandler(PaymentDbContext dbContext) : base(dbContext)
    {
    }
}
