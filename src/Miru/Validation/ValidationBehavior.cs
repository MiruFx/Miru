using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Miru.Validation;

public class ValidationBehavior<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidationBehavior<TRequest,TResponse>> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ValidationBehavior(
        ILogger<ValidationBehavior<TRequest,TResponse>> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }
        
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();
            
        if (validator != null)
        {
            var validationResult = await validator.ValidateAsync(request, ct);

            var failures = validationResult.Errors;
                
            if (failures.Any())
            {
                _logger.LogDebug(
                    $"{request.GetType().ActionName()} is an invalid request:{failures.Join2(x => $"{Environment.NewLine}\t{x}")}");
                    
                throw new MiruValidationException(request, failures);
            }
        }

        return await next();
    }
}