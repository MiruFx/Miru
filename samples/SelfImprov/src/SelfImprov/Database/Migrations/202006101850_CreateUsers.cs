using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace SelfImprov.Database.Migrations
{
    [Migration(202006101850)]
    public class CreateUsers : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsId()
                .WithColumn("Name").AsString(64)
                .WithColumn("Email").AsString(255)
                .WithColumn("HashedPassword").AsString(128)
                .WithTimeStamps();
        }
    }
}
