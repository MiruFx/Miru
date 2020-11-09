using System.Linq;
using Miru.Domain;
using Miru.Mvc;
using Miru.Pagination;
using Miru.Urls;

namespace Miru
{
    public static class PaginationExtensions
    {
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
    }
}