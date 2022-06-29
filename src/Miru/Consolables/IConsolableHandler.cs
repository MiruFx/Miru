using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Miru.Consolables;

public interface IConsolableHandler : ICommandHandler
{
    Task Execute();
            
    async Task<int> ICommandHandler.InvokeAsync(InvocationContext context)
    {
        await Execute();
        return 0;
    }

    int ICommandHandler.Invoke(InvocationContext context) => 0;
}