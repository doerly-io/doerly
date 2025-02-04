using Doerly.Module.Order.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace Doerly.Module.Order.Domain.Dtos.Requests.Order;

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
    public PaymentKind PaymentKind { get; set; }

    [Required]
    public DateTime DueDate { get; set; }

    [Required]
    public int CustomerId { get; set; }
}
