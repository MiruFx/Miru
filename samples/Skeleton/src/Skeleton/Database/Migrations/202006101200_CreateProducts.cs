using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Skeleton.Database.Migrations
{
    [Migration(202006101200)]
    public class CreateProducts : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Products")
                .WithColumn("Id").AsId()
                .WithColumn("Name").AsString(64);
        }
    }
}