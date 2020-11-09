using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Miru.Domain;

namespace Miru.Databases.EntityFramework
{
    public class TimeStampedBeforeSaveHandler : IBeforeSaveHandler
    {
        public void BeforeSaveChanges(DbContext db)
        {
            var entitiesBeingCreated = db.ChangeTracker.Entries<ITimeStamped>()
                .Where(p => p.State == EntityState.Added)
                .Select(p => p.Entity);

            foreach (var entityBeingCreated in entitiesBeingCreated)
            {
                entityBeingCreated.CreatedAt = DateTime.Now;
                entityBeingCreated.UpdatedAt = DateTime.Now;
            }

            var entitiesBeingUpdated = db.ChangeTracker.Entries<ITimeStamped>()
                .Where(p => p.State == EntityState.Modified)
                .Select(p => p.Entity);

            foreach (var entityBeingUpdated in entitiesBeingUpdated)
            {
                entityBeingUpdated.UpdatedAt = DateTime.Now;
            }
        }
    }
}