using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Miru.Storages;

public class AttachmentInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData @event, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var entries = @event.Context?.ChangeTracker.Entries();
        
        var modifiedEntries = entries
            .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified);

        foreach (var entry in modifiedEntries)
        {
            var attachmentIdProperties = entry.Properties
                .Where(p =>
                {
                    var foreignKeys = p.Metadata.GetContainingForeignKeys();

                    foreach (var foreignKey in foreignKeys)
                        if (foreignKey.PrincipalEntityType.ClrType == typeof(Attachment))
                            return true;

                    return false;
                })
                .ToList();

            foreach (var attachmentIdProperty in attachmentIdProperties)
            {
                var attachmentId = attachmentIdProperty.CurrentValue;
                
                // search for the Attachment in the entries of current session
                var attachmentEntry = entries
                    .FirstOrDefault(x => x.Properties.Any(p => 
                        x.Metadata.ClrType == typeof(Attachment) &&
                        p.Metadata.Name == "Id" && Convert.ToInt64(p.CurrentValue) == Convert.ToInt64(attachmentId)));
                
                if (attachmentEntry != null && attachmentEntry.Entity is Attachment attachment)
                {
                    attachment.Entity = entry.Metadata.ClrType.Name;
                    attachment.Property = attachmentIdProperty
                        .Metadata
                        .GetContainingForeignKeys()
                        .First()
                        .DependentToPrincipal?.Name;
                }
            }
        }

        return new ValueTask<InterceptionResult<int>>(result);
    }
}