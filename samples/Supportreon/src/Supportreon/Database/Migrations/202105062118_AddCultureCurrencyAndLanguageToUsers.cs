using FluentMigrator;
using Miru.Databases.Migrations.FluentMigrator;

namespace Supportreon.Database.Migrations
{
    [Migration(202105062118)]
    public class AddCultureCurrencyAndLanguageToUsers : AutoReversingMigration
    {
        public override void Up()
        {
            Alter.Table("Users")
                .AddColumn("Culture").AsString().Nullable()
                .AddColumn("Currency").AsString().Nullable()
                .AddColumn("Language").AsString().Nullable();
        }
    }
}
