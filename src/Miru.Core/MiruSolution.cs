using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Baseline;

namespace Miru.Core
{
    public class UnknownSolution : MiruSolution
    {
        public UnknownSolution() : base(MiruPath.CurrentPath, Assembly.GetEntryAssembly().GetName().Name)
        {
            RootDir = MiruPath.CurrentPath;
            CurrentDir = MiruPath.CurrentPath;

            SrcDir = CurrentDir;
            AppDir = CurrentDir;
        }
    }

    /// <summary>
    /// Contains directories and files information of a Miru solution
    /// </summary>
    public class MiruSolution
    {
        public static string ConfigYml(string environment = null) =>
            environment.IsNotEmpty() ? $"Config.{environment}.yml" : "Config.yml";
        
        public string Name { get;}
        
        /// <summary>
        /// Last part of a composed solution name, eg: "Microsoft.Dynamics" short name will be "Dynamics" 
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Current dir is the same as Directory.GetCurrentDirectory()
        /// </summary>
        public MiruPath CurrentDir { get; protected set; }
        
        public MiruPath RootDir { get; protected set; }
        public MiruPath SrcDir { get; protected set; }
        public MiruPath DatabaseDir { get; protected set; }
        public MiruPath MigrationsDir { get; protected set; }
        public MiruPath FabricatorsDir { get; protected set; }
        public string App { get; }
        public MiruPath AppDir { get; protected set; }
        public string AppTests { get; protected set; }
        public MiruPath AppTestsDir { get; protected set; }
        public string AppPageTests { get; protected set; }
        public MiruPath AppPageTestsDir { get; protected set; }
        public MiruPath FeaturesDir { get; protected set; }
        public MiruPath DomainDir { get; protected set; }
        public MiruPath TestsDir { get; protected set; }
        
        public MiruPath StorageDir { get; protected set; }

        public string AppBaseDir => AppContext.BaseDirectory;

        public MiruSolution(string rootDir, string appName = null)
        {
            CurrentDir = MiruPath.CurrentPath;
            RootDir = rootDir;
            Name = appName ?? Path.GetFileName(RootDir);
            ShortName = Name.Split(".").Last();
            
            SrcDir = RootDir / "src";
            TestsDir = RootDir / "tests";
            
            App = Name;
            AppDir = SrcDir / App;
            AppTests = $"{Name}.Tests";
            AppTestsDir = TestsDir / AppTests;
            AppPageTests = $"{Name}.PageTests";;
            AppPageTestsDir = TestsDir / AppPageTests;
            
            DatabaseDir = AppDir / "Database";
            MigrationsDir = DatabaseDir / "Migrations";
            FabricatorsDir = DatabaseDir / "Fabricators";
            FeaturesDir = AppDir / "Features";
            DomainDir = AppDir / "Domain";

            StorageDir = RootDir / "storage";
        }
        
        public string Relative(Func<MiruSolution, string> func)
        {
            var dir = func(this);
            
            return RootDir.Relative(dir);
        }
        
        public string Relative(string path)
        {
            return RootDir.Relative(path);
        }
    }
}