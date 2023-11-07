using Miru.Core.Process;

namespace Miru.Consolables;

public class AppSetupConsolable : Consolable
{
    public AppSetupConsolable()
        : base("app.setup", "Install/Update nuget/npm packages and run db migration and seed")
    {
    }

    public class ConsolableHandler : IConsolableHandler
    {
        private readonly MiruSolution _solution;

        public ConsolableHandler(MiruSolution solution)
        {
            _solution = solution;
        }

        public async Task Execute()
        {
            await RunCommand("dotnet", "restore");
            await RunCommand("dotnet", "build");
            await RunCommand("miru", "app npm install");
            await RunCommand("miru", "app npm run dev");
            await RunCommand("miru", "storage.link");
            await RunCommand("miru", "db.migrate");
            // await RunCommand("miru", "db.seed");
        }

        private async Task RunCommand(string executable, params string[] args)
        {
            var exec = OS.IsWindows 
                ? OS.WhereOrWhich(executable) 
                : executable;
        
            var processRunner = new MiruProcessRunner(false, string.Empty);

            var ct = new CancellationToken();
            
            var ret = await processRunner
                .RunAsync(new ProcessSpec()
                {
                    Executable = exec,
                    Arguments = args,
                    WorkingDirectory = _solution.RootDir
                })
                .WaitAsync(ct);

            if (ret != 0)
            {
                throw new MiruException("Process returned an error");
            }
        }
    }
}