using FluentMigrator;
using Miru.Behaviors.TimeStamp;
using Miru.Databases.Migrations.FluentMigrator;

namespace Corpo.Skeleton.Database.Migrations;
 
[Migration(999999999992)]
public class AlterCardsAddUserId : Migration
{
    public override void Up()
    {
        Alter.Table("Cards").AddColumn("Status").AsString();
    }

    public override void Down()
    {
        Delete.Column("Status").FromTable("Cards");
    }
}