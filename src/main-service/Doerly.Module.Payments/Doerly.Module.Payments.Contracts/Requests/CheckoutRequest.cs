using System.ComponentModel.DataAnnotations;
using Doerly.Module.Payments.Enums;

namespace Doerly.Module.Payments.Contracts;

public class CheckoutRequest
{
    public required decimal AmountTotal { get; set; }

    /// <summary>
    /// Id of user who will pay for the bill
    /// </summary>
    public required int PayerId { get; set; }

    [MinLength(5)]
    [MaxLength(50)]
    public required string Description { get; set; }

    /// <summary>
    /// Frontend url on which user will be returned from payment page
    /// </summary>
    public required string ReturnUrl { get; set; }

    /// <summary>
    /// Currency of the payment
    /// <para>UAH = 1</para>
    /// <para>USD = 2</para>
    /// <para>EUR = 3</para>
    /// </summary>
    [EnumDataType(typeof(ECurrency))]
    public required ECurrency Currency { get; set; }
    
}
