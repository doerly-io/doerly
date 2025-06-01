using Doerly.Module.Catalog.Enums;

namespace Doerly.Module.Catalog.Contracts.Dtos.Requests.Filter
{
    public class UpdateFilterRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public EFilterType Type { get; set; }
        public int CategoryId { get; set; }
    }
}
