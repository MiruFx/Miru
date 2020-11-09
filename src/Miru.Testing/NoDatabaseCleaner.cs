using System.Threading.Tasks;
using Miru.Databases;

namespace Miru.Testing
{
    public class NoDatabaseCleaner : IDatabaseCleaner
    {
        public Task Clear()
        {
            return Task.CompletedTask;
        }
    }
}