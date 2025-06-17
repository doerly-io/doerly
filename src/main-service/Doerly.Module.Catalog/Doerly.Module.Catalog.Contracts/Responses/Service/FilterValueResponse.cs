using Doerly.Module.Catalog.Enums;

namespace Doerly.Module.Catalog.Contracts.Responses
{
    public class FilterValueResponse
    {
        public int FilterId { get; set; }
        public string FilterName { get; set; } = null!;
        public EFilterType FilterType { get; set; }
        public string Value { get; set; } = null!;
    }
}
