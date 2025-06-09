using System.ComponentModel.DataAnnotations;
using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.DataTransferObjects.Requests;
public class UpdateOrderStatusRequest
{
    [Required(ErrorMessage = "FieldIsRequired")]
    public EOrderStatus Status { get; set; }

    [Url(ErrorMessage = "FieldIsUrl")]
    public string? ReturnUrl { get; set; }
}
