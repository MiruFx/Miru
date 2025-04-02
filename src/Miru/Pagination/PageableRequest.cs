using System.Collections.Generic;
using MediatR;

namespace Miru.Pagination;

public abstract class PageableRequest<TRequest, TItems> : IRequest<TRequest>, IPageable<TItems>
{
    public abstract IReadOnlyList<TItems> Results { get; set; }

    // paging
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Pages { get; set; }
    public int CountShowing { get; set; }
    public int CountTotal { get; set; }
}