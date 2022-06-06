using System.Threading.Tasks;

namespace Miru.Databases;

public interface IDataAccess
{
    Task SaveAsync(object[] entities);
}