using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Miru.Globalization;

public class GlobalizationBehavior<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly RequestLocalizationOptions _options;

    public GlobalizationBehavior(IOptions<RequestLocalizationOptions> options)
    {
        _options = options.Value;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        CultureInfo.CurrentCulture = _options.DefaultRequestCulture.Culture;
        CultureInfo.CurrentUICulture = _options.DefaultRequestCulture.UICulture;
            
        return await next();
    }
}