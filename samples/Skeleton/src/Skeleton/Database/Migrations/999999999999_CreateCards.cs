using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Skeleton.Database.Migrations
{
    [Migration(999999999999)]
    public class CreateCards : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("TableName")
                .WithColumn("Id").AsId()
                .WithColumn("Name").AsString(64);
        }
    }
}