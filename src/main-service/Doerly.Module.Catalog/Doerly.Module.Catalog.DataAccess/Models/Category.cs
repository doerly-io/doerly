using Doerly.DataAccess.Models;

namespace Doerly.Module.Catalog.DataAccess.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int? ParentId { get; set; }
        public Category? Parent { get; set; }
        public ICollection<Category> Children { get; set; } = new List<Category>();

        public bool IsDeleted { get; set; }
        public bool IsEnabled { get; set; }

        public virtual ICollection<Filter> Filters { get; set; } = new List<Filter>();
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
