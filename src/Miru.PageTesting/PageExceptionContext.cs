using System;

namespace Miru.PageTesting
{
    public class PageExceptionContext
    {
        public PageExceptionContext(MiruNavigator nav, Exception originalException, string failureMessage)
        {
            OriginalException = originalException;
            Nav = nav;
            FailureMessage = failureMessage;
        }

        public Exception OriginalException { get; }
        
        public MiruNavigator Nav { get; }
        
        public string FailureMessage { get; }
    }
}