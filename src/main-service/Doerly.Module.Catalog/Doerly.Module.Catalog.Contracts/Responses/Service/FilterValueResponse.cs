namespace Doerly.Module.Catalog.Contracts.Responses
{
    public class FilterValueResponse
    {
        public int FilterId { get; set; }
        public string FilterName { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
