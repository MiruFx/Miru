using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Miru.Domain;
using Miru.Mvc;
using Miru.Pagination;
using Miru.Urls;

namespace Miru
{
    public static class PaginationExtensions
    {
        public static Pager Pager(this IPageable pageable) =>
            pageable.Pager(PaginationConfig.DefaultPagerSize);

        public static Pager Pager(this IPageable pageable, int maxPagesToDisplay)
        {
            var firstPageToDisplay = 1;
            var lastPageToDisplay = pageable.Pages;

            if (maxPagesToDisplay > 0 && pageable.Pages > maxPagesToDisplay)
            {
                firstPageToDisplay = pageable.Page - maxPagesToDisplay / 2;

                if (firstPageToDisplay < 1)
                {
                    firstPageToDisplay = 1;
                }

                var pageNumbersToDisplay = maxPagesToDisplay;
                
                lastPageToDisplay = firstPageToDisplay + pageNumbersToDisplay - 1;

                if (lastPageToDisplay > pageable.Pages)
                {
                    lastPageToDisplay = pageable.Pages;
                }
            }
            
            return new Pager
            {
                Pages = Enumerable.Range(firstPageToDisplay, lastPageToDisplay - firstPageToDisplay + 1)
            };
        }
        
        public static IReadOnlyList<TModel> ToPaginate<TModel>(this IEnumerable<TModel> queryable, IPageable pageable)
        {
            var count = queryable.Count();
           
            pageable.Paginate(count);

            var result = queryable
                .Skip(pageable.Skip())
                .Take(pageable.PageSize)
                .ToList();

            pageable.CountShowing = result.Count;
            
            return result;
        }
        
        public static IReadOnlyList<TModel> ToPaginate<TModel>(this IQueryable<TModel> queryable, IPageable pageable)
        {
            var count = queryable.Count();
           
            pageable.Paginate(count);

            var result = queryable
                .Skip(pageable.Skip())
                .Take(pageable.PageSize)
                .ToList();

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
        
        public static bool HasPagination<T>(this IPageable<T> pageable)
        {
            return pageable.Pages > 1;
        }
        
        public static bool HasPreviousPage<T>(this IPageable<T> pageable)
        {
            return pageable.Results.Any() && pageable.Page > 1;
        }
        
        public static bool HasNextPage<T>(this IPageable<T> pageable)
        {
            return pageable.Results.Any() && pageable.Page < pageable.Pages;
        }

        public static UrlBuilder<T> PreviousPage<T>(this UrlBuilder<T> builder) where T : class, IPageable
        {
            return builder.With(x => x.Page, builder.Request.Page - 1);
        }
        
        public static UrlBuilder<T> NextPage<T>(this UrlBuilder<T> builder) where T : class, IPageable
        {
            return builder.With(x => x.Page, builder.Request.Page + 1);
        }
        
        public static UrlBuilder<T> FirstPage<T>(this UrlBuilder<T> builder) where T : class, IPageable
        {
            return builder.With(x => x.Page, 1);
        }
        
        public static UrlBuilder<T> LastPage<T>(this UrlBuilder<T> builder) where T : class, IPageable
        {
            return builder.With(x => x.Page, builder.Request.Pages);
        }
        
        public static UrlBuilder<T> Page<T>(this UrlBuilder<T> builder, int page) where T : class, IPageable
        {
            return builder.With(x => x.Page, page);
        }
    }
}