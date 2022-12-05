using FluentMigrator;
using Miru.Behaviors.TimeStamp;
using Miru.Databases.Migrations;
using Miru.Databases.Migrations.FluentMigrator;

namespace Corpo.Skeleton.Database.Migrations;
 
[Migration(999999999991)]
public class CreateCards : UpOnlyMigration
{
    public override void Up()
    {
        Create.Table("Cards")
            .WithId()
            .WithColumn("Name").AsString(64)
            .WithTimeStamps();
    }
}