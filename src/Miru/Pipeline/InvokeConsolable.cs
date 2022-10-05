using System;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Miru.Consolables;
using Miru.Core;

namespace Miru.Pipeline;

public class InvokeConsolable : Consolable
{
    public InvokeConsolable() : base("invoke", "Invokes a command")
    {
        Add(new Argument<string>("name"));
    }
            
    public class ConsolableHandler : IConsolableHandler
    {
        public string Name { get; set; }

        private readonly IMiruApp _app;

        public ConsolableHandler(IMiruApp app)
        {
            _app = app;
        }
                
        public async Task Execute()
        {
            var type = App.Assembly
                .ExportedTypes
                .SingleOrDefault(x => 
                    x.Name == "Command" 
                    && x.DeclaringType != null 
                    && x.DeclaringType.Name == Name);

            if (type == null )
            {
                Console2.RedLine($"Type {Name}.Command not found in assembly {App.Assembly.FullName}");
                return;
            }

            var instance = Activator.CreateInstance(type);

            if (instance is IBaseRequest request)
            {
                await _app.ScopedSendAsync(request);

                Console2.GreenLine("Done");

                return;
            }

            Console2.RedLine($"Type {Name}.Command is not a IRequest or IBaseRequest");
        }
    }
}