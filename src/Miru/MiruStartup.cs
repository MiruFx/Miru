using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Miru;

public abstract class MiruStartup
{
    public IConfiguration Configuration { get; }

    public IHostEnvironment Environment { get; }
    
    public MiruStartup(IHostEnvironment env, IConfiguration configuration)
    {
        Environment = env;
        Configuration = configuration;
    }
}