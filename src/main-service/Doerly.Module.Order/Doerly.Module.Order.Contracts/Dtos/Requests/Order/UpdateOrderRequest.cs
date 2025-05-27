using System.ComponentModel.DataAnnotations;
using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.Contracts.Dtos;
public class UpdateOrderRequest
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
}
