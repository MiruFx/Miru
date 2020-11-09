using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Mong.Database.Migrations
{
    [Migration(202005281800)]
    public class CreateProviders : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Providers")
                .WithId()
                .WithColumn("Name").AsString(64)
                .WithColumn("Amounts").AsString(64);
        }
    }
}