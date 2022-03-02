using System.IO;
using Miru.Core;
using Scripts.StubsExport;

namespace Scripts;

public class ExportStubs
{
    private static bool _exported;
    
    public static void Export()
    {
        if (_exported)
            return;
        
        var rootDir = new SolutionFinder().FromCurrentDir().Solution.RootDir;
        
        var param = new StubParams()
        {
            SkeletonDir = rootDir / "samples" / "Corpo.Skeleton",
            StubDir = rootDir / "src" / "Miru.Core" / "Templates"
        };
        
        Directories.DeleteIfExists(param.StubDir);
        Directory.CreateDirectory(param.StubDir);
            
        // FIXME: use Solution to build the artifacts' path
        new SolutionStubExport(param).Export();
        
        new CommandStubExport(param).Export();
        
        new QueryShowStubExport(param).Export();
        
        new QueryListStubExport(param).Export();
           
        new MigrationStubExport(param).Export();
        
        new EntityStubExport(param).Export();
        
        new ConsolableStubExport(param).Export();
        
        new JobStubExport(param).Export();
        
        new MailableStubExport(param).Export();
        
        new ConfigStubExport(param).Export();
        
        new FeatureScaffoldStubExport(param).Export();

        _exported = true;
    }
}