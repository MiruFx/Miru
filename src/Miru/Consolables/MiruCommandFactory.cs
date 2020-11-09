using System.Collections.Generic;
using Oakton;

namespace Miru.Consolables
{
    public class MiruCommandFactory : CommandFactory
    {
        public MiruCommandFactory(MiruCommandCreator commandCreator) : base(commandCreator)
        {
        }

        public override CommandRun HelpRun(Queue<string> queue)
        {
            var baseCommandRun = base.HelpRun(queue);
            
            baseCommandRun.Command = new MiruHelpCommand();

            return baseCommandRun;
        }
    }
}