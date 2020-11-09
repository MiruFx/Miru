using Serilog;

namespace Miru.Foundation.Logging
{
    class RegisteredLogger
    {
        public RegisteredLogger(ILogger logger)
        {
            Logger = logger;
        }
            
        public ILogger Logger { get; }
    }
}