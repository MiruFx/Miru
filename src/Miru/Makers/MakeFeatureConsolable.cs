using Baseline;
using Miru.Consolables;
using Miru.Core;
using Oakton;

namespace Miru.Makers
{
    [Description("Make a new Feature", Name = "make:feature")]
    public class MakeFeatureConsolable : ConsolableSync<MakeFeatureConsolable.Input>
    {
        private readonly MiruSolution _solution;

        public MakeFeatureConsolable(MiruSolution solution)
        {
            _solution = solution;
        }

        public class Input
        {
            public string In { get; set; }

            public string Name { get; set; }
            
            public string Action { get; set; }
            
            public bool NewFlag { get; set; }
            public bool EditFlag { get; set; }
            public bool ShowFlag { get; set; }
            public bool ListFlag { get; set; }
            
            [IgnoreOnCommandLine]
            public string Template {
                get
                {
                    if (NewFlag) return "New";
                    if (ShowFlag) return "Show";
                    if (ListFlag) return "List";
                    if (EditFlag) return "Edit";
                    return string.Empty;
                }}
        }
        
        public override bool Execute(Input input)
        {
            var make = new Maker(_solution);
            
            Console2.BreakLine();

            if (input.Template.IsEmpty())
            {
                Console2.YellowLine("Choose one of the templates passing the flag:");
                Console2.WhiteLine("--new");
                Console2.WhiteLine("--show");
                Console2.WhiteLine("--list");
                Console2.WhiteLine("--edit");
                
                return false;
            }
            
            make.Feature(input.In, input.Name, input.Action, input.Template);
            
            Console2.BreakLine();
            
            return true;
        }
    }
}