using System;
using System.Threading.Tasks;
using Hangfire;
using Miru.Consolables;

namespace Miru.Queuing
{
    public class QueueRunConsolable : Consolable
    {
        public QueueRunConsolable()
            : base("queue.run", "Run queue")
        {
        }

        public class ConsolableHandler : IConsolableHandler
        {
            private readonly IMiruApp _app;
            
            public ConsolableHandler(IMiruApp app)
            {
                _app = app;
            }

            public async Task Execute()
            {
                using var server = _app.Get<BackgroundJobServer>();
                
                Console.WriteLine("Queue is running. Press [Ctrl + C] or [Ctrl + Break] to exit...");

                await server.WaitForShutdownAsync(default);
            }
        }
    }
}