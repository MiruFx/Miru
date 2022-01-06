using Microsoft.EntityFrameworkCore;
using Miru.Scopables;

namespace SanusPlus.Infra.Scopables;

public abstract class Scopable<TDecorator> : Scopable
{
    public abstract void WhenSaving(TDecorator entity);
    
    public override void OnSaving(object entity)
    {
        if (entity is TDecorator typedEntity)
            WhenSaving(typedEntity);
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