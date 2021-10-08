using System.Threading.Tasks;
using Miru.Consolables;

namespace Miru.Databases.Migrations
{
    // #consolable
    public class DbRollbackConsolable : Consolable
    {
        public DbRollbackConsolable()
            : base("db.rollback", "Rollback database schema")
        {
        }

        public class ConsolableHandler : IConsolableHandler
        {
            public int Steps { get; set; } = 1;

            private readonly IDatabaseMigrator _databaseMigrator;

            public ConsolableHandler(IDatabaseMigrator databaseMigrator)
            {
                _databaseMigrator = databaseMigrator;
            }

            public Task Execute()
            {
                _databaseMigrator.DowngradeSchema(Steps);

                return Task.CompletedTask;
            }
        }
    }
    // #consolable
}
