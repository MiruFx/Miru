using Miru.Databases;

namespace Miru.Testing
{
    public class NoDatabaseCleaner : IDatabaseCleaner
    {
        public Task ClearAsync()
        {
            return Task.CompletedTask;
        }
    }
}