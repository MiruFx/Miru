using FluentMigrator.Builders.Create.Table;
using Miru.Databases.Migrations.FluentMigrator;

namespace Miru.Behaviors.UserStamp;

public static class UserStampMigrationExtensions
{
    public static ICreateTableWithColumnSyntax WithUserStamps(
        this ICreateTableWithColumnSyntax table,
        string columnCreatedBy = "CreatedById",
        string columnUpdatedBy = "UpdatedById",
        string userTable = "Users",
        bool nullable = false)
    {
        if (nullable)
            return table
                .WithColumn(columnCreatedBy).AsForeignKeyReference(userTable).Nullable()
                .WithColumn(columnUpdatedBy).AsForeignKeyReference(userTable).Nullable();
        
        return table
            .WithColumn(columnCreatedBy).AsForeignKeyReference(userTable)
            .WithColumn(columnUpdatedBy).AsForeignKeyReference(userTable);
    }
}