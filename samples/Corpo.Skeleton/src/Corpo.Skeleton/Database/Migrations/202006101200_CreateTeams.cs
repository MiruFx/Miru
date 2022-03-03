using FluentMigrator;
using Miru.Behaviors.TimeStamp;
using Miru.Databases.Migrations.FluentMigrator;

namespace Corpo.Skeleton.Database.Migrations;

[Migration(202006101200)]
public class CreateTeams : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("Teams")
            .WithColumn("Id").AsId()
            .WithColumn("Name").AsString(64)
            .WithTimeStamps();
    }
}
