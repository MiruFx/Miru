using System;
using System.Threading.Tasks;
using Hangfire;
using Miru.Consolables;
using Oakton;

namespace Miru.Queuing
{
    [Description("Run queue", Name = "queue:run")]
    public class QueueRunConsolable : Consolable
    {
        private readonly IMiruApp _app;

        public QueueRunConsolable(IMiruApp app)
        {
            _app = app;
        }

        public override async Task ExecuteAsync()
        {
            using (var server = _app.Get<BackgroundJobServer>())
            {
                Console.WriteLine("Queue is running. Press [Ctrl + C] or [Ctrl + Break] to exit...");

                await server.WaitForShutdownAsync(default);
            }
        }
    }
}