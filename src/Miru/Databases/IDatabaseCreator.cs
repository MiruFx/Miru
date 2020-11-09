using System.Threading.Tasks;

namespace Miru.Databases
{
    public interface IDatabaseCreator
    {
        Task Create();
    }
}