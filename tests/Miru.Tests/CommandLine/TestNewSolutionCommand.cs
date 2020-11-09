using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using Miru.Core;

namespace Miru.Tests.CommandLine
{
    public class TestNewSolutionCommand : Command
    {
        private readonly MiruPath _workingDir;
        private readonly MiruPath _newSolutionDir;
        private readonly MiruPath _sampleMongDir;

        private const string BasePath = "Miru";
        private const string SolutionName = "Mong";
        
        public TestNewSolutionCommand() : base(name: "test-new-solution")
        {
            _workingDir = A.TempPath() / BasePath;
            _newSolutionDir = _workingDir / SolutionName;
            _sampleMongDir = new SolutionFinder().FromCurrentDir().Solution.RootDir / "samples" / SolutionName;
            
            Handler = CommandHandler.Create(Execute);
        }

        public void Execute()
        {
            Console2.BreakLine();
            Console2.GreyLine($"Deleting folder {_newSolutionDir}");
            Console2.BreakLine();
            
            Directories.DeleteIfExists(_newSolutionDir);

            ExecuteShell($"miru new {SolutionName}", _workingDir);
            
            ExecuteShell("dotnet build --nologo", _newSolutionDir);

            ExecuteShell("miru assets:setup", _newSolutionDir);
            ExecuteShell("miru db:create", _newSolutionDir);
            ExecuteShell("miru db:create -e Test", _newSolutionDir);
            ExecuteShell("miru make:feature TopupNew Topups New", _newSolutionDir);
            ExecuteShell("miru make:entity Topup", _newSolutionDir);

            CopyFromSample(A.Path("src") / SolutionName / "Database" / "Migrations" / "201911071845_CreateTopup.cs");
            CopyFromSample(A.Path("src") / SolutionName / "Database" / "MongDbContext.cs");
            CopyFromSample(A.Path("src") / SolutionName / "Features" / "Topups" / "New.cshtml");
            CopyFromSample(A.Path("src") / SolutionName / "Features" / "Topups" / "TopupNew.cs");
            CopyFromSample(A.Path("src") / SolutionName / "Domain" / "Topup.cs");
            
            CopyFromSample(A.Path("tests") / $"{SolutionName}.Tests" / "Features" / "Topups" / "TopupNewTest.cs");
            CopyFromSample(A.Path("tests") / $"{SolutionName}.PageTests" / "Pages" / "Topups" / "TopupNewPageTest.cs");
            
            ExecuteShell("dotnet build  --nologo", _newSolutionDir);
            ExecuteShell("miru test", _newSolutionDir);
            ExecuteShell("miru pagetest", _newSolutionDir);
            
            Console2.GreenLine("YES! ");
        }

        private void ExecuteShell(string command, string workingDir = null)
        {
            Console2.BreakLine();
            Console2.YellowLine($"Executing '{command}' at {workingDir.Or("Current directory")}");
            Console2.BreakLine();
            
            int ret = OS.ShellToConsole(command, workingDir);

            if (ret != 0)
            {
                Console2.RedLine($"Shell command returned code {ret}. Something went wrong. It should return 0");
                throw new Exception($"Shell command returned code {ret}");
            }
        }

        private void CopyFromSample(string path)
        {
            File.Copy(_sampleMongDir / path, _newSolutionDir / path, true);
        }
    }
}