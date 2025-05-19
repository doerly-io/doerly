using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doerly.Module.Catalog.DataAccess.Constants
{
    internal class DbConstants
    {
        internal const string CatalogSchema = "catalog";

        internal class Tables
        {
            internal const string Service = "service";
            internal const string Category = "category";
            internal const string ServiceFilterValue = "service_filter_value";
            internal const string Filter = "filter";
        }
    }
}
