namespace Scripts.StubsExport;

public class FeatureScaffoldStubExport : StubExport
{
    public FeatureScaffoldStubExport(StubParams param) : base(param)
    {
    }

    public override void Export()
    {
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamNew.cs", 
            "Crud-New-Feature");
            
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamEdit.cs", 
            "Crud-Edit-Feature", 
            templateKey: "Edit");
        
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "Edit.cshtml", 
            "Crud-Edit-Feature.cshtml", 
            templateKey: "Edit");
        
        ExportFile(
            Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamEditTest.cs", 
            "Crud-Edit-FeatureTest", 
            templateKey: "Edit");
        
        ExportFile(
            Params.SkeletonDir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Teams" / "TeamEditPageTest.cs", 
            "Crud-Edit-FeaturePageTest", 
            templateKey: "Edit");
            
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamDelete.cs", 
            "Crud-Delete-Feature", 
            templateKey: "Delete");
        
        ExportFile(
            Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamDeleteTest.cs", 
            "Crud-Delete-FeatureTest", 
            templateKey: "Delete");
        
        // ExportFile(_dir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Teams" / "TeamDeletePageTest.cs", "Crud-Delete-FeaturePageTest", templateKey: "Delete");

        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "TeamList.cs", 
            "Crud-List-Feature", 
            templateKey: "List");
        
        ExportFile(
            Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Teams" / "List.cshtml", 
            "Crud-List-Feature.cshtml", 
            templateKey: "List");
        
        ExportFile(
            Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Teams" / "TeamListTest.cs", 
            "Crud-List-FeatureTest", 
            templateKey: "List");
        
        ExportFile(
            Params.SkeletonDir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Teams" / "TeamListPageTest.cs", 
            "Crud-List-FeaturePageTest", 
            templateKey: "List");
    }
}