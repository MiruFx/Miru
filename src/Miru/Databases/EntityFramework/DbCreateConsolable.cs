using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Databases.EntityFramework;

public class DbCreateConsolable : Consolable
{
    public DbCreateConsolable()
        : base("db.create", "Create database based on config.yml")
    {
    }

    public class ConsolableHandler : IConsolableHandler
    {
        private readonly IDatabaseCreator _databaseCreator;
            
        public ConsolableHandler(IDatabaseCreator databaseCreator)
        {
            _databaseCreator = databaseCreator;
        }

        public Task Execute()
        {
            Console2.Line("Creating database");

            _databaseCreator.Create().Wait();
         
            Console2.GreenLine("Database created");

            return Task.CompletedTask;
        }
    }
}