using Doerly.Domain.Models;

namespace Doerly.Module.Payments.Domain.Adapters;

public interface IPaymentAdapter
{
    Task<OperationResult> Adapt(string data, string? signature = null);
}
