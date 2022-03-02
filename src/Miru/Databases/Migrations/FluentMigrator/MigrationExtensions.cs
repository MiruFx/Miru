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
    
    // public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax AsForeignKeyReference(
    //     this ICreateTableColumnAsTypeSyntax column, 
    //     string foreignTable,
    //     string idColumn = "Id",
    //     bool indexed = true,
    //     bool deleteOnCascade = true)
    // {
    //     var attrs = column.AsInt64();
    //     
    //     if (indexed) 
    //         attrs = attrs.Indexed();
    //
    //     var attrs2 = attrs.ForeignKey(foreignTable, idColumn);
    //         
    //     return deleteOnCascade ? attrs2.OnDelete(Rule.Cascade) : attrs2;
    // }
    
    public static ICreateTableColumnOptionOrForeignKeyCascadeOrWithColumnSyntax AsForeignKeyReference(
        this ICreateTableColumnAsTypeSyntax column, 
        string foreignTable,
        string idColumn = "Id",
        bool deleteOnCascade = false)
    {
        var attrs = column.AsInt64().Indexed().ForeignKey(foreignTable, idColumn);
            
        return deleteOnCascade ? attrs.OnDelete(Rule.Cascade) : attrs;
    }
    
    public static IAlterTableColumnOptionOrAddColumnOrAlterColumnSyntax AsForeignKeyReference(
        this IAlterTableColumnAsTypeSyntax column, 
        string foreignTable,
        string idColumn = "Id",
        bool deleteOnCascade = false)
    {
        var attrs = column.AsInt64().Indexed().ForeignKey(foreignTable, idColumn);
            
        return deleteOnCascade ? attrs.OnDelete(Rule.Cascade) : attrs;
    }
}