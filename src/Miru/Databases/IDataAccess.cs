using System.Threading.Tasks;

namespace Miru.Databases
{
    public interface IDataAccess
    {
        Task PersistAsync(object[] entities);
    }
}
