using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using Hangfire;
using Miru.Consolables;

namespace Miru.Queuing;

// TODO: add automated tests
public class AppQueueRunConsolable : Consolable
{
    public AppQueueRunConsolable() : base("app.queue.run", "Run queue")
    {
        Add(new Option<string[]>(["--queues", "--queue", "-q"])
        {
            AllowMultipleArgumentsPerToken = true
        });
    }

    public class ConsolableHandler(IAppWarmup appWarmup, JobStorage jobStorage) 
        : IConsolableHandler
    {
        public string[] Queues { get; set; } = { "default:20" };

        public async Task Execute()
        {
            appWarmup.InitiateServices();

            // miru app.queue.run --queues default:15 default,exports:5 scheduled:1
            var servers = new List<BackgroundJobServer>();
            
            foreach (var queue in Queues)
            {
                var split = queue.Split(':');
                var queueNames = split[0].Split(',').ToArray();
                var workers = split[1];
                
                var options = new BackgroundJobServerOptions
                {
                    Queues = queueNames,
                    WorkerCount = workers.ToInt()
                };
                
                Console2.WhiteLine($"Adding a queue processor for {split[0]} queues with {workers} workers");
                
                servers.Add(new BackgroundJobServer(options, jobStorage));
            }

            var tasks = new List<Task>();

            foreach (var server in servers)
                tasks.Add(server.WaitForShutdownAsync(default));
            
            Console2.GreenLine("Queues are running");

            Task.WaitAll(tasks.ToArray());

            await Task.CompletedTask;
        }
    }
}