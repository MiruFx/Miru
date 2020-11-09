using Miru.Core;

namespace Miru.Tests
{
    public class MiruTestSolution : MiruSolution
    {
        public MiruTestSolution() : base(string.Empty)
        {
            var baseSolution = new SolutionFinder().FromCurrentDir().Solution;
            
            RootDir = baseSolution.TestsDir / "Miru.Tests";
            
            AppDir = RootDir;
            TestsDir = RootDir;
        }
    }
}