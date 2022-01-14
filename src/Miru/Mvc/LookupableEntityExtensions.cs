using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miru.Domain;

namespace Miru.Mvc
{
    public static class LookupableEntityExtensions
    {
        public static SelectLookups ToLookups<TEntity>(this IEnumerable<TEntity> list) 
            where TEntity : ILookupableEntity
        {
            var lookups = new SelectLookups();

            foreach (var item in list)
            {
                lookups.Add(new Lookup(item.Id, item.Name));
            }

            return lookups;
        }
        
        public static async Task<SelectLookups> ToLookupsAsync<TModel>(
            this IQueryable<TModel> queryable,
            CancellationToken ct = default) where TModel : ILookupableEntity
        {
            var result = await queryable
                .OrderBy(x => x.Name)
                .ToDictionaryAsync(x => x.Id, x => x.Name, ct);

            return result.ToSelectLookups();
        }
    }
}