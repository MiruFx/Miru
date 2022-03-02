using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Miru.Pagination;

public class PaginationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PaginationBehavior<TRequest, TResponse>> _logger;

    public PaginationBehavior(ILogger<PaginationBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var response = await next();
            
        if (request is IPageable pageable)
            _logger.LogDebug(string.Format(
                "Paging result: Page={0}, PageSize={1}, Pages={2}, Showing {3} of {4}",
                pageable.Page, 
                pageable.PageSize, 
                pageable.Pages, 
                pageable.CountShowing, 
                pageable.CountTotal));   
            
        return response;
    }
}