using FluentMigrator.Builders.Create.Table;

namespace Miru.Databases.Migrations.FluentMigrator;

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
        
    public static ICreateTableWithColumnSyntax WithId(this ICreateTableWithColumnSyntax table)
    {
        return table.WithColumn("Id").AsId();
    }
}