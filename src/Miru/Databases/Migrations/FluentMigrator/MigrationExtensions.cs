using System.Data;
using FluentMigrator.Builders.Alter.Table;
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
    
    public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax AsForeignKeyReference(
        this ICreateTableColumnAsTypeSyntax column, 
        string foreignTable,
        string idColumn = "Id",
        bool deleteOnCascade = true)
    {
        var attrs = column.AsInt64().ForeignKey(foreignTable, idColumn);
            
        return deleteOnCascade ? attrs.OnDelete(Rule.Cascade) : attrs;
    }
    
    public static IAlterTableAddColumnOrAlterColumnSyntax AsForeignKeyReference(
        this IAlterTableColumnAsTypeSyntax column, 
        string foreignTable,
        string idColumn = "Id",
        bool deleteOnCascade = true)
    {
        var attrs = column.AsInt64().ForeignKey(foreignTable, idColumn);
            
        return deleteOnCascade ? attrs.OnDelete(Rule.Cascade) : attrs;
    }
}