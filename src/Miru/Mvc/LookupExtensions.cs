using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Miru.Mvc
{
    public static class LookupExtensions
    {
        public static async Task<SelectLookups> ToLookupsAsync<TModel>(
            this IQueryable<TModel> queryable,
            Func<TModel, object> key,
            Func<TModel, object> value,
            CancellationToken ct = default)
        {
            return (await queryable.ToDictionaryAsync(key, value, ct)).ToSelectLookups();
        }
    }
}