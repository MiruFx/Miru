using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Miru.Databases.EntityFramework;

public class TransactionBehavior<TRequest, TResponse> : 
    IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly DbContext _db;

    public TransactionBehavior(DbContext db)
    {
        _db = db;
    }

    public async Task<TResponse> Handle(TRequest message, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(ct);
            
        try
        {
            var response = await next();
                    
            await _db.SaveChangesAsync(ct);
                    
            await transaction.CommitAsync(ct);
                    
            return response;
        }
        catch
        {
            await transaction.RollbackAsync(ct);
                
            throw;
        }
    }
}