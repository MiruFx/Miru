using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Miru.Scopables;

public class ScopableBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly DbContext _db;
    private readonly IEnumerable<IScopableQuery> _filters;

    public ScopableBehavior(DbContext db, IEnumerable<IScopableQuery> filters)
    {
        _db = db;
        _filters = filters;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        foreach (var queryFilter in _filters)
        {
            queryFilter.WhenQuerying(_db);
        }
       
        return await next();
    }
}