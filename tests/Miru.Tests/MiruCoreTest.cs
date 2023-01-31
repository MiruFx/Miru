using System;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Tests;

public class MiruCoreTest : IDisposable
{
    private bool _initialized;
    private readonly Action<IServiceCollection> _servicesConfig;
    private ServiceProvider _sp;

    public MiruCoreTest()
    {
    }

    public MiruCoreTest(Action<IServiceCollection> servicesConfig)
    {
        _servicesConfig = servicesConfig;

        OneTimeSetup();
    }

    public TestFixture _ => _sp.GetService<TestFixture>();
    
    public DisposableTestFixture DisposableTestFixture => new(_sp.Get<IMiruApp>(), _sp);
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        if (_initialized)
            return;
        
        var services = new ServiceCollection()
            .AddMiruCoreTesting();

        if (_servicesConfig != null)
            _servicesConfig(services);
        else
            ConfigureServices(services);
            
        _sp = services.BuildServiceProvider();
           
        _initialized = true;
    }

    public virtual IServiceCollection ConfigureServices(IServiceCollection services) => services;

    public void Dispose()
    {
        _sp.Get<IMiruApp>().Dispose();
        _sp.Dispose();
    }
}