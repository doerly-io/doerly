using Doerly.Domain;
using Doerly.Module.Order.Enums;
using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Order.Contracts.Dtos;

public class CreateOrderRequest
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
