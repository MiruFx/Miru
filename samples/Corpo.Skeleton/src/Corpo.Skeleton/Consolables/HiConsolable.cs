using System;
using System.Threading.Tasks;
using Miru.Consolables;

namespace Corpo.Skeleton.Consolables
{
    // #consolable
    public class HiConsolable : Consolable
    {
        public HiConsolable() : base("hi")
        {
        }

        public class ConsolableHandler : IConsolableHandler
        {
            public Task Execute()
            {
                throw new NotImplementedException();
            }
        }
        
        public void Execute()
        {
            Console.WriteLine("Hi!");
        }
    }
    // #consolable
}