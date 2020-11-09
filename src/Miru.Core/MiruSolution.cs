using System;
using System.IO;
using System.Reflection;
using Baseline;

namespace Miru.Core
{
    public class UnknownSolution : MiruSolution
    {
        public UnknownSolution() : base(MiruPath.CurrentPath, Assembly.GetCallingAssembly().GetName().Name)
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
        /// Current dir is the same as Directory.GetCurrentDirectory()
        /// </summary>
        public MiruPath CurrentDir { get; protected set; }
        
        public MiruPath RootDir { get; protected set; }
        public MiruPath SrcDir { get; protected set; }
        public MiruPath ConfigDir { get; protected set; }
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
            
            SrcDir = A.Path(RootDir, "src");
            ConfigDir = A.Path(RootDir, "config");
            TestsDir = A.Path(RootDir, "tests");
            
            App = Name;
            AppDir = A.Path(SrcDir, App);
            AppTests = $"{Name}.Tests";
            AppTestsDir = A.Path(TestsDir, AppTests);
            AppPageTests = $"{Name}.PageTests";;
            AppPageTestsDir = A.Path(TestsDir, AppPageTests);
            
            DatabaseDir = A.Path(AppDir, "Database");
            MigrationsDir = A.Path(DatabaseDir, "Migrations");
            FabricatorsDir = A.Path(DatabaseDir, "Fabricators");
            FeaturesDir = A.Path(AppDir, "Features");
            DomainDir = A.Path(AppDir, "Domain");

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
        
        public string GetConfigYml(string environment)
        {
            return Path.Combine(ConfigDir, $"Config.{environment}.yml");
        }
    }
}