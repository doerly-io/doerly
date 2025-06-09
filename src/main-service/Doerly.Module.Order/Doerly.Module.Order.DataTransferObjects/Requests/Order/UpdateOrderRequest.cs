using System.ComponentModel.DataAnnotations;
using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.DataTransferObjects.Requests;
public class UpdateOrderRequest
{

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

    public bool UseProfileAddress { get; set; }

    public int? RegionId { get; set; }

    public int? CityId { get; set; }
}
