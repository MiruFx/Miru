using Microsoft.EntityFrameworkCore;

namespace Miru.Scopables;

public abstract class Scopable<TDecorator> : Scopable
{
    public abstract void WhenSaving(TDecorator entity);
    
    public override void OnSaving(object entity)
    {
        if (entity is TDecorator typedEntity)
            this.WhenSaving(typedEntity);
    }
}

public abstract class Scopable : IScopableSaving, IScopableQuery
{
    public virtual void OnSaving(object entity)
    {
    }

    public virtual void WhenQuerying(DbContext db)
    {
    }
}