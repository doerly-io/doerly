using Doerly.Module.Profile.DataTransferObjects.Profile;

namespace Doerly.Module.Order.DataTransferObjects.Responses;

public class GetOrdersWithPaginationAndFilterResponse
{
    public int OrderId { get; set; }

    public int? CategoryId { get; set; }

    public string Name { get; set; }

    public decimal Price { get; set; }

    // public bool IsPriceNegotiable { get; set; }

    public DateTime DueDate { get; set; }

    public ProfileInfo Customer { get; set; }

    public DateTime CreatedDate { get; set; }
}
