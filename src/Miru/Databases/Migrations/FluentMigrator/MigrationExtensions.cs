using FluentMigrator.Builders.Create.Table;

namespace Miru.Databases.Migrations.FluentMigrator
{
    public static class MigrationExtensions
    {
        public static ICreateTableWithColumnSyntax AsId(this ICreateTableColumnAsTypeSyntax column)
        {
            return column.AsInt64().NotNullable().PrimaryKey().Identity();
        }
        
        public static ICreateTableWithColumnSyntax AsReference(this ICreateTableColumnAsTypeSyntax column)
        {
            return column.AsInt64().NotNullable();
        }
        
        public static ICreateTableWithColumnSyntax WithTimeStamps(this ICreateTableWithColumnSyntax table)
        {
            return table
                .WithColumn("CreatedAt").AsDateTime()
                .WithColumn("UpdatedAt").AsDateTime();
        }
        
        public static ICreateTableWithColumnSyntax WithId(this ICreateTableWithColumnSyntax table)
        {
            return table.WithColumn("Id").AsId();
        }
    }
}