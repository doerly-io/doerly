using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.DataAccess.Models;

namespace Doerly.Module.Order.DataAccess.Entities;
public class OrderFile : BaseEntity
{
    public int OrderId { get; set; }

    public string Path { get; set; }

    public string Name { get; set; }

    public long Size { get; set; }

    public string Type { get; set; }

    public virtual Order Order { get; set; }

}
