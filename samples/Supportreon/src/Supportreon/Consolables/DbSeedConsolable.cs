using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;
using Miru.Databases.EntityFramework;
using Oakton;
using Supportreon.Database;
using Supportreon.Domain;

namespace Supportreon.Consolables
{
    [Description("Seed database", Name = "db:seed")]
    public class DbSeedConsolable : Consolable<DbSeedConsolable.Input>
    {
        private readonly SupportreonDbContext _db;

        public DbSeedConsolable(SupportreonDbContext db)
        {
            _db = db;
        }

        public class Input
        {
        }

        public override async Task<bool> Execute(Input input)
        {
            Console2.Line("Seeding data");
            
            _db.Categories.AddIfNotExists(
                m => m.Name == "Arts", new Category { Name = "Arts" });
            
            _db.Categories.AddIfNotExists(
                m => m.Name == "Food", new Category { Name = "Food" });
            
            _db.Categories.AddIfNotExists(
                m => m.Name == "Music", new Category { Name = "Music" });

            await _db.SaveChangesAsync();
            
            Console2.GreenLine("Done!");

            return true;
        }
    }
}
