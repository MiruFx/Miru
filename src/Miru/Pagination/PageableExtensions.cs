using System;

namespace Miru.Pagination;

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
    
    public static bool HasPagination(this IPageable pageable)
    {
        return pageable.Pages > 1;
    }
        
    public static bool HasPreviousPage(this IPageable pageable)
    {
        return pageable.CountTotal > 0 && pageable.Page > 1;
    }
        
    public static bool HasNextPage(this IPageable pageable)
    {
        return pageable.CountTotal > 0 && pageable.Page < pageable.Pages;
    }
}