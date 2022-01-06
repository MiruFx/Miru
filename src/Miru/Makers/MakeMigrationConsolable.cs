using System.CommandLine;
using System.Threading.Tasks;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers;

public class MakeMigrationConsolable : Consolable
{
    public MakeMigrationConsolable() :
        base("make.migration", "Make a Database Migration")
    {
        Add(new Argument<string>("name"));
    }

    public class ConsolableHandler : IConsolableHandler
    {
        private readonly Maker _maker;
    
        public string Name { get; set; }
        
        public ConsolableHandler(Maker maker)
        {
            _maker = maker;
        }
    
        public async Task Execute()
        {
            Console2.BreakLine();
            
            _maker.Migration(Name);
    
            Console2.BreakLine();
            
            await Task.CompletedTask;
        }
    }
}