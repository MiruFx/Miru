using System.Data;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Miru.Databases.EntityFramework
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly DbContext _db;
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

        public TransactionBehavior(DbContext db, ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest message, CancellationToken ct, RequestHandlerDelegate<TResponse> next)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync(ct);
            
            _logger.LogDebug($"Started transaction #{transaction.GetHashCode()}");

            try
            {
                var response = await next();
                    
                await _db.SaveChangesAsync(ct);
                    
                await transaction.CommitAsync(ct);
                    
                _logger.LogDebug($"Committed transaction #{transaction.GetHashCode()}");
                    
                return response;
            }
            catch
            {
                _logger.LogDebug($"Rollback transaction #{transaction.GetHashCode()}");
                
                await transaction.RollbackAsync(ct);
                
                throw;
            }
        }
    }
}