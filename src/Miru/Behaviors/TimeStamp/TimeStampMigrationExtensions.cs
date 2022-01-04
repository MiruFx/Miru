using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create.Table;

namespace Miru.Behaviors.TimeStamp;

public static class TimeStampMigrationExtensions
{
    public static ICreateTableWithColumnSyntax WithTimeStamps(
        this ICreateTableWithColumnSyntax table,
        string columnCreatedAt,
        string columnUpdatedAt)
    {
        return table
            .WithColumn(columnCreatedAt).AsDateTime()
            .WithColumn(columnUpdatedAt).AsDateTime();
    }
        
    public static ICreateTableWithColumnSyntax WithTimeStamps(
        this ICreateTableWithColumnSyntax table)
    {
        return table.WithTimeStamps("CreatedAt", "UpdatedAt");
    }
}