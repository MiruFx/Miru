using System.Threading.Tasks;
using Miru.Consolables;

namespace Miru.Databases.Migrations
{
    public class DbResetConsolable : Consolable
    {
        public DbResetConsolable()
            : base("db.reset", "Reset database schema. Downgrade to version 0 and migrate up")
        {
        }

        public class ConsolableHandler : IConsolableHandler
        {
            private readonly IDatabaseMigrator _databaseMigrator;
            
            public ConsolableHandler(IDatabaseMigrator databaseMigrator)
            {
                _databaseMigrator = databaseMigrator;
            }

            public Task Execute()
            {
                _databaseMigrator.RecreateSchema();

                return Task.CompletedTask;
            }
        }
    }
}
