using System.IO;
using Miru.Core;

namespace Scripts.StubsExport;

public class SolutionStubExport : StubExport
{
    public SolutionStubExport(StubParams param) : base(param)
    {
    }

    public override void Export()
    {
        Params.SolutionMap = true;
        
         // New
        ExportFile(Params.SkeletonDir / "Corpo.Skeleton.sln", "Solution.sln");
        ExportFile(Params.SkeletonDir / "gitignore", ".gitignore", destinationFile: ".gitignore");
        ExportFile(Params.SkeletonDir / "README.md");
                    
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "webpack.mix.js");
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Startup.cs");
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Corpo.Skeleton.csproj");
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Program.cs");
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "GlobalUsing.cs");
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "package.json");
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "appSettings-example.yml");
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "appSettings.yml");

        ExportDir(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Config");
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Database" / "Migrations" / "202001290120_CreateUsers.cs");
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Database" / "AppDbContext.cs");
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Domain" / "User.cs");
        
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "_ViewImports.cshtml");
        ExportFile(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "_ViewStart.cshtml");
        ExportDir(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Accounts");
        ExportDir(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Home");
        ExportDir(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "Features" / "Shared");
        ExportDir(Params.SkeletonDir / "src" / "Corpo.Skeleton" / "resources");
        
        ExportFile(Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Corpo.Skeleton.Tests.csproj");
        ExportFile(Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "AppFabricator.cs");
        ExportFile(Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Program.cs", "'Program");
        ExportFile(Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "GlobalUsing.cs", "'GlobalUsing");
        ExportFile(Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Extensions.cs");
        ExportDir(Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Accounts");
        ExportDir(Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Features" / "Home");
        ExportDir(Params.SkeletonDir / "tests" / "Corpo.Skeleton.Tests" / "Config");
        
        ExportFile(Params.SkeletonDir / "tests" / "Corpo.Skeleton.PageTests" / "Corpo.Skeleton.PageTests.csproj");
        ExportFile(Params.SkeletonDir / "tests" / "Corpo.Skeleton.PageTests" / "Program.cs", "''Program");
        ExportDir(Params.SkeletonDir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Accounts");
        ExportDir(Params.SkeletonDir / "tests" / "Corpo.Skeleton.PageTests" / "Pages" / "Home");
        ExportDir(Params.SkeletonDir / "tests" / "Corpo.Skeleton.PageTests" / "Config");
        
        SaveMapForNewSolution();
    }
    
    private void SaveMapForNewSolution()
    {
        File.WriteAllText(Params.StubDir / "_New.yml", Yml.ToYml(NewSolutionFiles));
            
        NewSolutionFiles.Clear();
            
        Params.SolutionMap = false;
    }
}