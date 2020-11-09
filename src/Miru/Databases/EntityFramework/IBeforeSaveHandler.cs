using Microsoft.EntityFrameworkCore;

namespace Miru.Databases.EntityFramework
{
    public interface IBeforeSaveHandler
    {
        void BeforeSaveChanges(DbContext db);
    }
}