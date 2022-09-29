using FluentMigrator;

namespace Miru.Databases.Migrations;

public abstract class UpOnlyMigration : Migration
{
    public override void Down()
    {
    }
}