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

    public string FilePath { get; set; }

    public string FileName { get; set; }

    public long FileSize { get; set; }

    public string FileType { get; set; }

    public virtual Order Order { get; set; }

}
