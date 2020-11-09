using System;
using System.Threading.Tasks;
using Miru.Consolables;
using Oakton;

namespace Skeleton.Consolables
{
    // #consolable
    [Description("Description what this consolable does", Name = "hi")]
    public class HiConsolable : Consolable<HiConsolable.Input>
    {
        public class Input
        {
        }

        public override Task<bool> Execute(Input input)
        {
            Console.WriteLine("Hi!");
            
            return Task.FromResult(true); 
        }
    }
    // #consolable
}