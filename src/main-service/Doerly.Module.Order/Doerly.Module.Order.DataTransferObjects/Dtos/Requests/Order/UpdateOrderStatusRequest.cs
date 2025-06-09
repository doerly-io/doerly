using System.ComponentModel.DataAnnotations;

using Doerly.Domain;
using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.DataTransferObjects.Dtos;
public class UpdateOrderStatusRequest
{
    [Required(ErrorMessage = "FieldIsRequired")]
    public EOrderStatus Status { get; set; }

    [Url(ErrorMessage = "FieldIsUrl")]
    public string? ReturnUrl { get; set; }
}
