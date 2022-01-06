using System;

namespace Scripts.StubsExport;

public class MigrationStubExport : StubExport
{
    public MigrationStubExport(StubParams param) : base(param)
    {
    }

    public override void Export()
    {
        Func<string, string> tokens = line => line
            .Replace("999999999991", "{{ input.Version }}")
            .Replace("999999999992", "{{ input.Version }}")
            .Replace("CreateCards", "{{ input.Name }}")
            .Replace("AlterCardsAddUserId", "{{ input.Name }}")
            .Replace("TableName", "{{ input.Table }}")
            .Replace("ColumnName", "{{ input.Column }}");
            
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Database" / "Migrations" / "999999999991_CreateCards.cs", 
            "Migration", 
            tokens: tokens);
        
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Database" / "Migrations" / "999999999992_AlterCardsAddUserId.cs", 
            "MigrationAlter", 
            tokens: tokens);
    }
}