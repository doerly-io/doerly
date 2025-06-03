using Doerly.Module.Catalog.Enums;

namespace Doerly.Module.Catalog.Contracts.Responses
{
    public class GetFilterResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public EFilterType Type { get; set; }
        public List<string>? Options { get; set; }
        public int CategoryId { get; set; }
    }
}
