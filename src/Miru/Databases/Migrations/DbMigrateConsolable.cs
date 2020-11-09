using Miru.Consolables;
using Oakton;

namespace Miru.Databases.Migrations
{
    [Description("Update database schema", Name = "db:migrate")]
    public class DbMigrateConsolable : ConsolableSync
    {
        private readonly IDatabaseMigrator _databaseMigrator;

        public DbMigrateConsolable(IDatabaseMigrator databaseMigrator)
        {
            _databaseMigrator = databaseMigrator;
        }

        public override void Execute()
        {
            _databaseMigrator.UpdateSchema();
        }
    }
}