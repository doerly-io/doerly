namespace Doerly.Module.Catalog.Contracts.Requests
{
    public class CreateServiceRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public decimal? Price { get; set; }

        public Dictionary<int, string> FilterValues { get; set; } = new();
    }
}
