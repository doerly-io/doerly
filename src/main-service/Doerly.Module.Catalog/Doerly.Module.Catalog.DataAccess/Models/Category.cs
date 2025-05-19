using Doerly.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ICollection<Service> Services { get; set; } = new List<Service>();
    }
}
