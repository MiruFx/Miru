using System.Threading.Tasks;
using Miru.Consolables;

namespace Miru.Databases.Migrations
{
    public class DbMigrateConsolable : Consolable
    {
        public DbMigrateConsolable()
            : base("db.migrate", "Update database schema")
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
                _databaseMigrator.UpdateSchema();

                return Task.CompletedTask;
            }
        }
    }
}