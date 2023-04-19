using System.Data;
using FluentMigrator.Builders.Alter.Column;
using FluentMigrator.Builders.Alter.Table;
using FluentMigrator.Builders.Create.Table;

namespace Miru.Databases.Migrations.FluentMigrator;

public static class ForeignKeyReferenceExtensions
{
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
    
    public static IAlterColumnOptionOrForeignKeyCascadeSyntax AsForeignKeyReference(
        this IAlterColumnAsTypeOrInSchemaSyntax column, 
        string foreignTable,
        string idColumn = "Id",
        bool deleteOnCascade = false)
    {
        var attrs = column.AsInt64().Indexed().ForeignKey(foreignTable, idColumn);
            
        return deleteOnCascade ? attrs.OnDelete(Rule.Cascade) : attrs;
    }
}