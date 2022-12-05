using Miru.Core;

namespace Miru.Testing.Html;

public class MiruTestSolution : MiruSolution
{
    public MiruTestSolution() : base(string.Empty)
    {
        var baseSolution = new SolutionFinder().FromCurrentDir().Solution;
            
        RootDir = baseSolution.RootDir;
        StorageDir = baseSolution.StorageDir;
    }
}