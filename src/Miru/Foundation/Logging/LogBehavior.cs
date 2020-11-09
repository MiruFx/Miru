using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Miru.Foundation.Logging
{
    public class LogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LogBehavior<TRequest, TResponse>> _logger;

        public LogBehavior(ILogger<LogBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestName = request.GetType().ToString();
            var timer = new Stopwatch();
            timer.Start();

            try
            {
                _logger.LogInformation($"Processing {requestName}");
                var response = await next();
                return response;
            }
            catch (ValidationException)
            {
                // ValidationException is logged in ValidationBehavior
                // Here we are catching and doing nothing to not log the stacktrace and make log cleaner
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occured while handling {requestName}");
                throw;
            }
            finally
            {
                timer.Stop();
                _logger.LogInformation($"Processed {requestName} in {timer.ElapsedMilliseconds} ms");
            }
        }
    }
}