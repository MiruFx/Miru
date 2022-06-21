using System;
using Serilog;

namespace Miru.Foundation.Logging;

public class AppLoggerFactory
{
    private readonly Func<ILogger> _func;

    public AppLoggerFactory(Func<ILogger> func)
    {
        _func = func;
    }

    public ILogger GetLogger()
    {
        return _func();
    }
}