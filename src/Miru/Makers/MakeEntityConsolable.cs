using System.CommandLine;
using System.Threading.Tasks;
using Humanizer;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Makers;

public class MakeEntityConsolable : Consolable
{
    public MakeEntityConsolable() :
        base("make.entity", "Make an Entity")
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
            
            _maker.Entity(Name);
    
            Console2.BreakLine();
            
            Console2.GreyLine("\tConsider adding to your DbContext:");
            Console2.BreakLine();
            Console2.WhiteLine($"\t\tpublic DbSet<{Name}> {Name.Pluralize()} {{ get; set; }}");
            
            await Task.CompletedTask;
        }
    }
}