using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Miru.Testing.Html;

public abstract class MiruCoreTesting
{
    protected TestFixture _;
    
    [OneTimeSetUp]
    public void SetupMiruCoreTesting()
    {
        var services = new ServiceCollection()
            .AddMiruCoreTesting();
            
        ConfigureServices(services);
            
        _ = services
            .BuildServiceProvider()
            .GetService<TestFixture>();
    }

    public virtual IServiceCollection ConfigureServices(IServiceCollection services) => services;
}