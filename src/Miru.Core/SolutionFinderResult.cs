namespace Miru.Core
{
    public class SolutionFinderResult
    {
        public static SolutionFinderResult Empty => new SolutionFinderResult();

        public SolutionFinderResult(MiruSolution solution = null)
        {
            Solution = solution;
        }
        
        public bool FoundSolution => Solution != null;
        
        public MiruSolution Solution { get; }
    }
}