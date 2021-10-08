using System.Threading.Tasks;
using Miru.Consolables;

namespace Corpo.Skeleton.Consolables
{
    public class SeedConsolable : Consolable
    {
        public SeedConsolable() 
            : base("seed", "Description what this consolable does")
        {
        }

        public class ConsolableHandler : IConsolableHandler
        {
            public Task Execute()
            {
                return Task.CompletedTask;
            }
        }
    }
}