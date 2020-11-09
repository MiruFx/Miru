using System.Threading.Tasks;

namespace Miru.Databases
{
    public interface IDatabaseCleaner
    {
        Task Clear();
    }
}