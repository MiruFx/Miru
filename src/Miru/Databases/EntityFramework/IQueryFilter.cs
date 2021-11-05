using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public interface IQueryFilter
{
    void Apply(DbContext dbContext);
}

public class QueryFiltersInterceptor : DbConnectionInterceptor
{
    public IEnumerable<IQueryFilter> Filters { get; set; }

    public QueryFiltersInterceptor(IEnumerable<IQueryFilter> queryFilters)
    {
        Filters = queryFilters;
    }
}
