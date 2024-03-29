using System.Diagnostics;

namespace Miru.Testing;

[DebuggerStepThrough]
public class DisposableTestFixture : TestFixture, IDisposable
{
    private readonly ServiceProvider _serviceProvider;

    public DisposableTestFixture(IMiruApp app, ServiceProvider serviceProvider) : base(app)
    {
        _serviceProvider = serviceProvider;
    }
        
    public void Dispose()
    {
        _serviceProvider.Dispose();
    }
}