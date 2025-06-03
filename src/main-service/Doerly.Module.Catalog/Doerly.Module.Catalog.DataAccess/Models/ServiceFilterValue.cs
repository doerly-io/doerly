using Doerly.DataAccess.Models;

namespace Doerly.Module.Catalog.DataAccess.Models
{
    public class ServiceFilterValue : BaseEntity
    {
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        public int FilterId { get; set; }
        public Filter Filter { get; set; } = null!;

        public string Value { get; set; } = null!;
    }
}
