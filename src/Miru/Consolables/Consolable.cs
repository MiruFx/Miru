using System.Threading.Tasks;
using Oakton;

namespace Miru.Consolables
{
    public abstract class ConsolableSync<TInput> : OaktonCommand<TInput>, IConsolable
    {
    }
    
    public abstract class ConsolableSync : OaktonCommand<ConsolableInput>, IConsolable
    {
        public abstract void Execute();
        
        public override bool Execute(ConsolableInput input)
        {
            Execute();
            return true;
        }
    }
    
    public abstract class Consolable<TInput> : OaktonAsyncCommand<TInput>, IConsolable
    {
    }
    
    public abstract class Consolable : OaktonAsyncCommand<ConsolableInput>, IConsolable
    {
        public abstract Task Execute();
        
        public override async Task<bool> Execute(ConsolableInput input)
        {
            await Execute();
            return true;
        }
    }

    public class ConsolableInput
    {
    }
}