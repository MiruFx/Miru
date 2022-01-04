using FluentMigrator.Builders.Create.Table;

namespace Miru.Behaviors.UserStamp;

public static class UserStampMigrationExtensions
{
    public static ICreateTableWithColumnSyntax WithUserStamps(
        this ICreateTableWithColumnSyntax table,
        string columnCreatedAt,
        string columnUpdatedAt)
    {
        return table
            .WithColumn(columnCreatedAt).AsDateTime()
            .WithColumn(columnUpdatedAt).AsDateTime();
    }
        
    public static ICreateTableWithColumnSyntax WithUserStamps(
        this ICreateTableWithColumnSyntax table)
    {
        return table.WithUserStamps("CreatedById", "UpdatedById");
    }
}