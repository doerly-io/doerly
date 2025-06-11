namespace Doerly.Module.Catalog.Contracts.Responses
{
    public class FilterValueRequest
    {
        public int FilterId { get; set; }
        public string Value { get; set; } = null!;
    }
}
