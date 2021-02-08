using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Miru.Validation
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidator<TRequest> _validator;
        private readonly ILogger<ValidationBehavior<TRequest,TResponse>> _logger;

        public ValidationBehavior(
            ValidatorFactory validatorFactory, 
            ILogger<ValidationBehavior<TRequest,TResponse>> logger)
        {
            _logger = logger;
            _validator = validatorFactory.ValidatorFor<TRequest>();
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken ct, RequestHandlerDelegate<TResponse> next)
        {
            if (_validator != null)
            {
                var validationResult = await _validator.ValidateAsync(request, ct);

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
}