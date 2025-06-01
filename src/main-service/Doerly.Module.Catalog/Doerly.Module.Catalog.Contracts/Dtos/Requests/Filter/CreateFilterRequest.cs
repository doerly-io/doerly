using Doerly.Module.Catalog.Enums;

namespace Doerly.Module.Catalog.Contracts.Dtos.Requests.Filter
{
    public class CreateFilterRequest
    {
        public string Name { get; set; } = null!;
        public EFilterType Type { get; set; }
        public int CategoryId { get; set; }
    }
}
