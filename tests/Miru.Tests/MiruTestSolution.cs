namespace Miru.Tests;

public class MiruTestSolution : MiruSolution
{
    public MiruTestSolution() : base(string.Empty)
    {
        var baseSolution = new SolutionFinder().FromCurrentDir().Solution;
            
        // RootDir = baseSolution.TestsDir / "Miru.Tests";
        RootDir = baseSolution.RootDir;
        StorageDir = baseSolution.StorageDir;
        
        // AppDir = RootDir;
        // TestsDir = RootDir;
    }
}