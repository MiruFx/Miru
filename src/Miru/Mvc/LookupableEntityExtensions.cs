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
        public static Lookups ToLookups<TEntity>(this IEnumerable<TEntity> list) 
            where TEntity : ILookupableEntity
        {
            var lookups = new Lookups();

            foreach (var item in list)
            {
                lookups.Add(new Lookup(item.Id, item.Name));
            }

            return lookups;
        }
        
        public static async Task<Lookups> ToLookupsAsync<TModel>(
            this IQueryable<TModel> queryable,
            CancellationToken ct = default) where TModel : ILookupableEntity
        {
            var result = await queryable
                .OrderBy(x => x.Name)
                .ToDictionaryAsync(x => x.Id, x => x.Name, ct);

            return result.ToLookups();
        }
    }
}