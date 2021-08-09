using System;
using System.Collections.Generic;
using System.Linq;

namespace Miru.Pagination
{
    public static class PageableExtensions
    {
        public static void Paginate(this IPageable pageable, int totalCount)
        {
            var page = pageable.Page > 0 
                ? pageable.Page 
                : 1;
            
            var pageSize = pageable.PageSize > 0 
                ? pageable.PageSize 
                : PaginationConfig.DefaultPageSize;
            
            var pages = (int) Math.Ceiling((double) totalCount / pageSize);

            pageable.Page = page;
            pageable.PageSize = pageSize;
            pageable.Pages = pages;
            pageable.CountTotal = totalCount;
        }

        public static int Skip(this IPageable pageable)
        {
            return (pageable.Page - 1) * pageable.PageSize;
        }
    }

    public class Pager
    {
        public IEnumerable<int> Pages { get; set; }
    }
}