using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Supportreon.Database.Migrations
{
    [Migration(202102171514)]
    public class AddIsAdminToUsers : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Users").AddColumn("IsAdmin").AsBoolean().Nullable();
        }
    }
}
