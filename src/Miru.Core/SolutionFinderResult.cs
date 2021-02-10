namespace Miru.Core
{
    public class SolutionFinderResult
    {
        public static SolutionFinderResult Empty => new(null, string.Empty);

        public SolutionFinderResult(MiruSolution solution, string lookedAt)
        {
            Solution = solution;
            LookedAt = lookedAt;
        }

        public string LookedAt { get; }

        public bool FoundSolution => Solution != null;
        
        public MiruSolution Solution { get; }
    }
}