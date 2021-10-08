using System.CommandLine;

namespace Miru.Consolables
{
    public abstract class Consolable : Command
    {
        protected Consolable(string name, string description = null) : base(name, description)
        {
        }
    }
}