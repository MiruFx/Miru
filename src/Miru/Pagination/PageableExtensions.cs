using System;
using System.Collections.Generic;
using System.Linq;

namespace Miru.Pagination
{
    public static class PageableExtensions
    {
        public static void Paginate(this IPageable pageable, int totalCount)
        {
            var page = pageable.Page > 0 ? pageable.Page : 1;
            
            var pageSize = pageable.PageSize > 0 ? pageable.PageSize : PaginationConfig.DefaultPageSize;
            
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
                    firstPageToDisplay = pageable.Pages - maxPagesToDisplay + 1;
                }
            }
            
            return new Pager
            {
                Pages = Enumerable.Range(firstPageToDisplay, lastPageToDisplay - firstPageToDisplay + 1)
            };
        }
    }

    public class Pager
    {
        public IEnumerable<int> Pages { get; set; }
    }
}