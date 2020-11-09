using System;

namespace Miru.Tests
{
    public interface IMiruTestWebHost : IDisposable
    {
        void Run(Action<IServiceProvider> action);
    }
}