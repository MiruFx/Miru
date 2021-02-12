using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Supportreon.Database.Migrations
{
    [Migration(202007032214)]
    public class CreateCategory : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Categories")
                .WithColumn("Id").AsId()
                .WithColumn("Name").AsString(64);
        }
    }
}
