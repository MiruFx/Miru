using System;
using Microsoft.Extensions.Hosting;
using Miru.Consolables;
using Oakton;
using Quartz;

namespace Miru.Scheduling
{
    [Description("Run schedule", Name = "schedule:run")]
    public class ScheduleRunConsolable : ConsolableSync
    {
        private readonly IMiruApp _app;

        public ScheduleRunConsolable(IMiruApp app)
        {
            _app = app;
        }

        public override void Execute()
        {
            var server = _app.Get<IHostedService>();
            server.StartAsync(default);
            Console.WriteLine("Queue is running. Press [Ctrl + C] or [Ctrl + Break] to exit...");
            server.StopAsync(default);
        }
    }
}