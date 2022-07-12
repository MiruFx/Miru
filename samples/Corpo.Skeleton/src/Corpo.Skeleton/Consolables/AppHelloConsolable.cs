using Miru.Consolables;

namespace Corpo.Skeleton.Consolables;

// #consolable
public class AppHelloConsolable : Consolable
{
    public AppHelloConsolable() : base("app.hello", "Renders Hello")
    {
    }

    public class ConsolableHandler : IConsolableHandler
    {
        public Task Execute()
        {
            Console.WriteLine("Hello!");

            return Task.CompletedTask;
        }
    }
}
// #consolable