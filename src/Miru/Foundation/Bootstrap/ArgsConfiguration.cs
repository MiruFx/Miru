using System.Collections.Generic;
using Miru.Core;

namespace Miru.Foundation.Bootstrap;

public class ArgsConfiguration
{
    public string Environment { get; private set; }
        
    public bool Verbose { get; private set; }
        
    public bool IsRunningCli { get; }
    
    public bool IsRunningWebApp => IsRunningCli == false;
        
    public string[] CliArgs { get; } = { };
        
    public ArgsConfiguration(string[] args)
    {
        if (args.Length > 0)
        {
            IsRunningCli = args[0].CaseCmp("miru");
            CliArgs = GetCliArgs(args);
        }
    }

    private string[] GetCliArgs(string[] args)
    {
        var cliArgs = new List<string>(args.Length);

        for (int i = 0; i < args.Length; i++)
        {
            if (i == 0 && args[i].CaseCmp("miru"))
            {
                continue;
            }

            if (args[i].CaseCmp("--environment", "-e") && i + 1 < args.Length)
            {
                i++;
                Environment = args[i++];
                continue;
            }

            if (args[i].CaseCmp("--verbose"))
            {
                Verbose = true;
                continue;
            }

            // remove last argument in case parameter is a project assembly
            // jetbrains rider add automatically when debugging
            // to debug miru cli, project assembly in the end will throw a parser error on oakton
            if (i == args.Length - 1 && args[i].Contains("bin/Debug") && args[i].Contains(".dll"))
                continue;

            cliArgs.Add(args[i]);
        }

        return cliArgs.ToArray();
    }
}