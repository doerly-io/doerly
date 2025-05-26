using Doerly.Domain;
using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.Contracts.Dtos;
public class UpdateOrderStatusRequest
{
    public EOrderStatus Status { get; set; }

    public string? ReturnUrl { get; set; }
}
