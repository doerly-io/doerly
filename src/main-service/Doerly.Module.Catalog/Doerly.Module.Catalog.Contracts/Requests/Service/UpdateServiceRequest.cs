namespace Doerly.Module.Catalog.Contracts.Requests
{
    public class UpdateServiceRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsEnabled { get; set; }

        public List<FilterValueRequest> FilterValues { get; set; } = new();
    }
}
