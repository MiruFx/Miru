using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Mong.Database.Migrations
{
    [Migration(202011200130)]
    public class CreateTeam : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Teams")
                .WithColumn("Id").AsId()
                .WithColumn("Name").AsString(64);
        }
    }
}
