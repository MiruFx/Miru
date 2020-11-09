using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Miru.Databases.EntityFramework
{
    public abstract class MiruDbContext : DbContext
    {
        private readonly IEnumerable<IBeforeSaveHandler> _preSaveHandlers;
        
        protected MiruDbContext(
            DbContextOptions options,
            IEnumerable<IBeforeSaveHandler> preSaveHandlers) : base(options)
        {
            _preSaveHandlers = preSaveHandlers;
        }
        
        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            foreach (var preSaveHandler in _preSaveHandlers)
            {
                preSaveHandler.BeforeSaveChanges(this);
            }

            return await base.SaveChangesAsync(ct);
        }
    }
}