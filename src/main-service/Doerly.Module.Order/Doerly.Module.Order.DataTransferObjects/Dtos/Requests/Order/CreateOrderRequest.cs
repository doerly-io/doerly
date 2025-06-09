using Doerly.Domain;
using Doerly.Module.Order.Enums;

using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Order.DataTransferObjects.Dtos;

public class CreateOrderRequest
{
    [Required(ErrorMessage = "FieldIsRequired")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "FieldIsRequired")]
    [MinLength(1, ErrorMessage = "FieldIsTooShort")]
    [MaxLength(100, ErrorMessage = "FieldIsTooLong")]
    public string Name { get; set; }

    [Required(ErrorMessage = "FieldIsRequired")]
    [MinLength(5, ErrorMessage = "FieldIsTooShort")]
    [MaxLength(4000, ErrorMessage = "FieldIsTooLong")]
    public string Description { get; set; }

    [Required(ErrorMessage = "FieldIsRequired")]
    [Range(1, 9999999999999.99, ErrorMessage = "FieldIsInRange")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "FieldIsRequired")]
    public EPaymentKind PaymentKind { get; set; }

    [Required(ErrorMessage = "FieldIsRequired")]
    public DateTime DueDate { get; set; }

    [Required(ErrorMessage = "FieldIsRequired")]
    public bool IsPriceNegotiable { get; set; }

    [Required(ErrorMessage = "FieldIsRequired")]
    public bool UseProfileAddress { get; set; }

    public int? RegionId { get; set; }

    public int? CityId { get; set; }

    public int? ExecutorId { get; set; }
}
