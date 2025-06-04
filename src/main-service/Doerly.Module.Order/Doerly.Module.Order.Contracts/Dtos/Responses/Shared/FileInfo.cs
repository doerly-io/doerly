using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Order.Contracts.Dtos;

public class FileInfo
{
    public string FilePath { get; set; }

    public string FileName { get; set; }

    public long FileSize { get; set; }

    public string FileType { get; set; }
}
