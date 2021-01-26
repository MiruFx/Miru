using System.Threading.Tasks;
using Miru.Consolables;
using Oakton;

namespace Corpo.Skeleton.Consolables
{
    [Description("Description what this consolable does", Name = "seed")]
    public class SeedConsolable : Consolable<SeedConsolable.Input>
    {
        public class Input
        {
        }

        public override Task<bool> Execute(Input input)
        {
            // do the task
            
            return Task.FromResult(true); 
        }
    }
}