namespace Doerly.Module.Catalog.Contracts.Requests
{
    public class FilterValueRequest
    {
        public int FilterId { get; set; }
        public string Value { get; set; } = null!;
    }
}
