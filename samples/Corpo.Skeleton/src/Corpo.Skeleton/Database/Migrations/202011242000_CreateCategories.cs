using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Corpo.Skeleton.Database.Migrations;

[Migration(202011242000)]
public class CreateCategories : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("Categories")
            .WithColumn("Id").AsId()
            .WithColumn("Name").AsString(64);
    }
}