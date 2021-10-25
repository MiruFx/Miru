using System;
using System.Threading.Tasks;
using Miru.Consolables;

namespace Corpo.Skeleton.Consolables
{
    // #consolable
    public class HiConsolable : Consolable
    {
        public HiConsolable() : base("hi", "Renders hi")
        {
        }

        public class ConsolableHandler : IConsolableHandler
        {
            public Task Execute()
            {
                Console.WriteLine("Hi!");

                return Task.CompletedTask;
            }
        }
    }
    // #consolable
}