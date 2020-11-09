using Miru.Core;
using Oakton;

namespace Miru.Consolables
{
    // [Description("Install front-end assets (npm install)", Name = "assets:install")]
    // public class AssetsInstallConsolable : ConsolableSync
    // {
    //     private readonly MiruSolution _solution;
    //
    //     public AssetsInstallConsolable(MiruSolution solution)
    //     {
    //         _solution = solution;
    //     }
    //
    //     public override void Execute()
    //     {
    //         Console2.YellowLine($"Running 'npm install' on {_solution.Relative(m => m.AppDir)}");
    //         Console2.White($"It can take some minutes");
    //         
    //         OS.ShellToConsole("npm install --no-progress");
    //     }
    // }
}