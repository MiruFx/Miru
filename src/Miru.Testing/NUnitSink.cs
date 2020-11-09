using System;
using System.IO;
using NUnit.Framework;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Miru.Testing
{
    public class NUnitSink : ILogEventSink
    {
        private readonly MessageTemplateTextFormatter _formatter;

        public NUnitSink(MessageTemplateTextFormatter formatter)
        {
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            _formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            if (TestContext.Out != null)
            {
                var writer = new StringWriter();
                _formatter.Format(logEvent, writer);

                TestContext.Progress.Write(writer.ToString());
            }
        }
    }
}