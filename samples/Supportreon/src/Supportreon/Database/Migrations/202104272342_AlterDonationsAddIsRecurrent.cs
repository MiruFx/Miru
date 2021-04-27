using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Supportreon.Database.Migrations
{
    [Migration(202104272342)]
    public class AlterDonationsAddIsRecurrent : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Donations").AddColumn("IsRecurrent").AsBoolean().Nullable();
        }
    }
}
