using Doerly.Module.Catalog.Enums;

namespace Doerly.Module.Catalog.Contracts.Requests
{
    public class CreateFilterRequest
    {
        public string Name { get; set; } = null!;
        public EFilterType Type { get; set; }
        public int CategoryId { get; set; }
    }
}
