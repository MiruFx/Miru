using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;
using Oakton;

namespace Miru.Databases.EntityFramework
{
    [Description("Create database based on config.yml", Name = "db:create")]
    public class DbCreateConsolable : Consolable
    {
        private readonly IDatabaseCreator _databaseCreator;

        public DbCreateConsolable(IDatabaseCreator databaseCreator)
        {
            _databaseCreator = databaseCreator;
        }

        public override async Task Execute()
        {
            Console2.Line("Creating database");

            await _databaseCreator.Create();
         
            Console2.GreenLine("Database created");
        }
    }
}