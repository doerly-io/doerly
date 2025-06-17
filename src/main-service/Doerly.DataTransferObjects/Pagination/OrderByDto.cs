using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Doerly.DataAccess.Models;

namespace Doerly.DataTransferObjects.Pagination;

public class OrderByDto<TEntity> where TEntity : BaseEntity
{
    public Expression<Func<TEntity, object>> Expression { get; set; }

    public bool IsDescending { get; set; } = false;
}
