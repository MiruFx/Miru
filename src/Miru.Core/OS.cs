using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Humanizer;

namespace Miru.Core;

public class OS
{
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        
    public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        
    public static bool IsOSX => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        
    /// <remarks>
    /// Usually,
    ///     for linux: /home/{{ user }}/.dotnet/tools
    ///     for windows: C:\Users\{{ user }}\.dotnet\tools
    /// </remarks>
    public static string DotNetToolsPath => 
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".dotnet", "tools");

    public static string ShellScriptExtension => IsWindows ? "cmd" : "sh";
        
    public static string ShellName => IsWindows ? "cmd" : "/bin/bash";
        
    public static string ShellArguments => IsWindows ? "/c {0}" : "-c \"{0}\"";
        
    public static string BuildShellArgs(string args) => IsWindows ? $"/c {args}" : $"-c \"{args}\"";

    /// <summary>
    /// Executes a Shell command redirecting the output to the Console. If OS is windows, executes "cmd.exe /c ARGS",
    /// otherwise executes /bin/bash -c ARGS 
    /// </summary>
    public static int ShellToConsole(string command, string workingDir = null)
    {
        var procStartInfo = new ProcessStartInfo(ShellName, string.Format(ShellArguments, command))
        {
            WorkingDirectory = workingDir ?? MiruPath.CurrentPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var proc = new System.Diagnostics.Process { StartInfo = procStartInfo };

        proc.OutputDataReceived += (sender, eventArgs) => Console.WriteLine(eventArgs.Data);
        proc.ErrorDataReceived += (sender, eventArgs) => Console.WriteLine(eventArgs.Data);

        proc.Start();

        proc.BeginOutputReadLine();
        proc.BeginErrorReadLine();

        proc.WaitForExit();

        return proc.ExitCode;
    }

    /// <summary>
    /// It executes 'where' on Windows or 'which' on Linux/OSx and returns where
    /// the command is
    /// </summary>
    public static string WhereOrWhich(string command)
    {
        using var process = new System.Diagnostics.Process
        {
            StartInfo =
            {
                UseShellExecute = false, 
                RedirectStandardOutput = true, 
                FileName = IsWindows ? "where" : "which",
                Arguments = command
            }
        };
            
        process.Start();

        var output = process.StandardOutput.ReadToEnd();
            
        var commandLocation = IsWindows
            ? output.Split(Environment.NewLine).FirstOrDefault(x => x.EndsWith(".exe") || x.EndsWith(".cmd"))
            : output;
            
        process.WaitForExit();

        return commandLocation;
    }
}