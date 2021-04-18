using System.Collections.Generic;

namespace Miru.Pagination
{
    public interface IPageable
    {
        int Page { get; set; }
        
        int PageSize { get; set; }
        
        int Pages { get; set; }
        
        int CountShowing { get; set; }
        
        int CountTotal { get; set; }
    }

    public interface IPageable<out TItem> : IPageable
    {
        IReadOnlyList<TItem> Results { get; }
    }
}