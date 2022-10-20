using System;
using System.Diagnostics;

namespace Miru.Testing;

[DebuggerStepThrough]
public class TestFixture : ITestFixture
{
    public IMiruApp App { get; }

    public TestFixture(IMiruApp app)
    {
        App = app;
    }
        
    public T Get<T>()
    {
        return App.Get<T>();
    }
        
    public object Get(Type type)
    {
        return App.Get(type);
    }
}