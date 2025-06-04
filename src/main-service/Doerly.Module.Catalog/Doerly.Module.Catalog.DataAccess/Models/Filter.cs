using Doerly.DataAccess.Models;
using Doerly.Module.Catalog.Enums;

namespace Doerly.Module.Catalog.DataAccess.Models
{
    public class Filter : BaseEntity
    {
        public string Name { get; set; } = null!;
        public EFilterType Type { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<ServiceFilterValue> FilterValues { get; set; } = new List<ServiceFilterValue>();
    }
}
