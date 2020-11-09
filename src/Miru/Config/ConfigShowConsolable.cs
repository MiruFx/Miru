using Microsoft.Extensions.Hosting;
using Miru.Consolables;
using Miru.Core;
using Miru.Settings;
using Oakton;

namespace Miru.Config
{
    [Description("Show the configuration will be used", Name = "config:show")]
    public class ConfigShowConsolable : ConsolableSync
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly DatabaseOptions _dbOptions;

        public ConfigShowConsolable(IHostEnvironment hostEnvironment, DatabaseOptions dbOptions)
        {
            _hostEnvironment = hostEnvironment;
            _dbOptions = dbOptions;
        }

        public override void Execute()
        {
            Console2.YellowLine("Host:");
            Console2.Line(_hostEnvironment.ToYml());

            Console2.YellowLine("Database:");
            Console2.Line(_dbOptions.ToYml());
        }
    }
}