using Doerly.Domain.Models;

namespace Doerly.Module.Payments.Domain.Adapters;

public interface IPaymentAdapter
{
    Task<HandlerResult> Adapt(string data, string? signature = null);
}
