using FluentMigrator;
using Miru.Behaviors.TimeStamp;
using Miru.Databases.Migrations.FluentMigrator;

namespace Corpo.Skeleton.Database.Migrations;

[Migration(202303312001)]
public class CreateTodos : AutoReversingMigration
{
    public override void Up()
    {
        Create.Table("Todos")
            .WithColumn("Id").AsId()
            .WithColumn("Name").AsString(64)
            .WithColumn("UserId").AsForeignKeyReference("Users")
            .WithTimeStamps();
    }
}
