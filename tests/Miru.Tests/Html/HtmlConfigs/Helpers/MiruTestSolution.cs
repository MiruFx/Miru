namespace Miru.Tests.Html.HtmlConfigs.Helpers;

public class MiruTestSolution : MiruSolution
{
    public MiruTestSolution() : base(string.Empty)
    {
        var baseSolution = new SolutionFinder().FromCurrentDir().Solution;
            
        RootDir = baseSolution.RootDir;
        StorageDir = baseSolution.StorageDir;
    }
}