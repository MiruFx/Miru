using Microsoft.EntityFrameworkCore;

namespace Miru.Scopables;

public interface IScopableQuery
{
    void WhenQuerying(DbContext db);
}