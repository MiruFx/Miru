using Miru.Consolables;
using Oakton;

namespace Miru.Databases.Migrations
{
    // #consolable
    [Description("Rollback database schema", Name = "db:rollback")]
    public class DbRollbackConsolable : ConsolableSync<DbRollbackConsolable.Input>
    {
        public class Input
        {
            [Description("Number of steps that will be downgraded")]
            public int Steps { get; } = 1;
        }
        
        private readonly IDatabaseMigrator _databaseMigrator;

        public DbRollbackConsolable(IDatabaseMigrator databaseMigrator)
        {
            _databaseMigrator = databaseMigrator;
            
            Usage("Rollback to [steps] previous migration").Arguments(m => m.Steps);
            Usage("Rollback to the previous migration").Arguments();
        }

        public override bool Execute(Input input)
        {
            _databaseMigrator.DowngradeSchema(input.Steps);

            return true;
        }
    }
    // #consolable
}
