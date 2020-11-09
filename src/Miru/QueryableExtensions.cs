using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Baseline;
using HtmlTags;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Miru.Domain;
using Miru.Pagination;

namespace Miru
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereWhen<T>(this IQueryable<T> queryable, bool when, Expression<Func<T, bool>> where)
        {
            return when ? queryable.Where(where) : queryable;
        }
        
        public static IQueryable<T> WhereWhenNot<T>(this IQueryable<T> queryable, bool whenNot, Expression<Func<T, bool>> where)
        {
            return whenNot ? queryable : queryable.Where(where);
        }
        
        public static async Task<TEntity> ByIdAsync<TEntity>(
            this IQueryable<TEntity> query, 
            long id,
            CancellationToken ct = default) where TEntity : IEntity
        {
            return await query.FirstOrDefaultAsync(e => e.Id == id, ct);
        }
        
        public static async Task<EntityEntry<TEntity>> RemoveByIdAsync<TEntity>(
            this DbSet<TEntity> dbSet, long id) where TEntity : class, IEntity
        {
            var entity = await dbSet.SingleOrFailAsync(e => e.Id == id);
     
            return dbSet.Remove(entity);
        }
        
        public static IQueryable<TEntity> ByIds<TEntity>(this IQueryable<TEntity> query, IEnumerable<long> ids) where TEntity : Entity
        {
            return query.Where(p => ids.Contains(p.Id));
        }
        
        public static Task<List<TEntity>> ByIdsAsync<TEntity>(this IQueryable<TEntity> query, IEnumerable<long> ids) where TEntity : Entity
        {
            return query.Where(p => ids.Contains(p.Id)).ToListAsync();
        }
        
        public static async Task<TSource> SingleOrFailAsync<TSource>(
            this IQueryable<TSource> queryable,
            string exceptionMessage,
            CancellationToken cancellationToken = default)
        {
            return await queryable.SingleOrFailAsync(null, exceptionMessage, cancellationToken);
        }
        
        public static async Task<TSource> SingleOrFailAsync<TSource>(
            this IQueryable<TSource> queryable,
            Expression<Func<TSource, bool>> predicate = null,
            string exceptionMessage = null,
            CancellationToken cancellationToken = default)
        {
            TSource found;
            
            if (predicate == null)
                found = await queryable.SingleOrDefaultAsync(cancellationToken);
            else
                found = await queryable.SingleOrDefaultAsync(predicate, cancellationToken);
            
            if (found == null)
                throw new NotFoundException(exceptionMessage.Or($"Query could not find any result retrieving {typeof(TSource).Name}"));

            return found;
        }
        
        /// <summary>
        /// Retrieves an Entity by Id or throw a NotFoundException if not found
        /// </summary>
        /// <exception cref="NotFoundException"></exception>
        public static TEntity ByIdOrFail<TEntity>(this IQueryable<TEntity> queryable, long id) where TEntity : IEntity
        {
            return queryable.FirstOrDefault(e => e.Id == id) ?? 
                   throw new NotFoundException($"{typeof(TEntity)} with Id #{id} could not be found");
        }
        
        /// <summary>
        /// Retrieves an Entity by Id or throw a NotFoundException if not found
        /// </summary>
        /// <exception cref="NotFoundException"></exception>
        public static async Task<TEntity> ByIdOrFailAsync<TEntity>(
            this IQueryable<TEntity> queryable, 
            long id) where TEntity : IEntity
        {
            return await queryable.ByIdOrFailAsync(id, default);
        }
        
        public static async Task<TEntity> ByIdOrFailAsync<TEntity>(
            this IQueryable<TEntity> queryable, 
            long id,
            CancellationToken ct) where TEntity : IEntity
        {
            return await queryable.FirstOrDefaultAsync(e => e.Id == id, ct) 
                   ?? 
                   throw new NotFoundException($"{typeof(TEntity).Name} with Id #{id} could not be found");
        }
        
        public static async Task<TEntity> ByIdOrFailAsync<TEntity>(
            this IQueryable<TEntity> queryable, 
            long id, 
            string exceptionMessage,
            CancellationToken ct) where TEntity : IEntity
        {
            return await queryable.FirstOrDefaultAsync(e => e.Id == id, cancellationToken: ct) ?? 
                   throw new NotFoundException(exceptionMessage.Or($"{typeof(TEntity).Name} with Id #{id} could not be found"));
        }

        public static IEnumerable<TModel> ToPaginate<TModel>(this IQueryable<TModel> queryable, IPageable pageable)
        {
            var count = queryable.Count();
           
            pageable.Paginate(count);

            var result = queryable.Skip(pageable.Skip()).Take(pageable.PageSize).ToList();

            pageable.CountShowing = result.Count;
            
            return result;
        }
        
        public static async Task<List<TModel>> ToPaginateAsync<TModel>(
            this IQueryable<TModel> queryable, 
            IPageable pageable,
            CancellationToken ct = default)
        {
            var total = queryable.Count();

            pageable.Paginate(total);

            var result = await queryable
                .Skip(pageable.Skip())
                .Take(pageable.PageSize)
                .ToListAsync(ct);

            pageable.CountShowing = result.Count;
            
            return result;
        }
        
        public static async Task<bool> NoneAsync<TSource>(
            this IQueryable<TSource> query, 
            Expression<Func<TSource, bool>> predicate,
            CancellationToken ct = default)
        {
            return await query.CountAsync(predicate, ct) == 0;
        }
        
        public static async Task<bool> UniqueAsync<TEntity>(
            this IQueryable<TEntity> queryable, 
            long id,
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default) where TEntity : IEntity
        {
            return await queryable
                .Where(m => m.Id != id)
                .CountAsync(predicate, ct) == 0;
        }
        
        public static async Task<bool> UniqueAsync<TEntity>(
            this IQueryable<TEntity> queryable, 
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default) where TEntity : IEntity
        {
            return await queryable.CountAsync(predicate, ct) == 0;
        }
        
        public static async Task<IEnumerable<ILookupable>> ToLookupableAsync<TEntity>(
            this IQueryable<TEntity> queryable,
            CancellationToken ct = default) where TEntity : ILookupable
        {
            return (await queryable.ToListAsync(ct)).Cast<ILookupable>();
        }
        
        public static HtmlTag ToSelectOptionsTags<TSource>(
            this IEnumerable<TSource> query, 
            Func<TSource, object> valueAndText = null)
        {
            var tag = new HtmlTag("option").Text(string.Empty).Attr("value", string.Empty);
            
            query.Each(m =>
            {
                var text = valueAndText != null ? valueAndText(m) : m;
                
                var option = new HtmlTag("option")
                    .Text(text.ToString())
                    .Attr("value", text.ToString());
                
                tag.Append(option);
            });

            return tag;
        }
    }
}