using System.ComponentModel.DataAnnotations;
using Doerly.Domain;
using Doerly.Module.Order.Enums;

namespace Doerly.Module.Order.Contracts.Dtos;
public class UpdateOrderRequest
{
    [Required]
    public int CategoryId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public EPaymentKind PaymentKind { get; set; }

    [Required]
    public DateTime DueDate { get; set; }
}
