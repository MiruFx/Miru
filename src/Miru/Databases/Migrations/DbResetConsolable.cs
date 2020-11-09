using Miru.Consolables;
using Oakton;

namespace Miru.Databases.Migrations
{
    [Description("Reset database schema. Downgrade to version 0 and migrate up", Name = "db:reset")]
    public class DbResetConsolable : ConsolableSync
    {
        private readonly IDatabaseMigrator _databaseMigrator;

        public DbResetConsolable(IDatabaseMigrator databaseMigrator)
        {
            _databaseMigrator = databaseMigrator;
        }

        public override void Execute()
        {
            _databaseMigrator.RecreateSchema();
        }
    }
}
