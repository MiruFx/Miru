using System;
using Miru.Core;
using Miru.Makers;
using Miru.Testing;
using NUnit.Framework;

namespace Miru.Tests.Makers
{
    public class AppSettingsMakerTest
    {
        private MiruPath _solutionDir;

        [SetUp]
        [TearDown]
        public void Setup()
        {
            _solutionDir = A.TempPath / "Miru" / "Pantanal";
            
            Console.WriteLine(_solutionDir);
            
            Directories.DeleteIfExists(_solutionDir);
        }
        
        [Test]
        public void Skip_if_destination_app_settings_exist()
        {
            // arrange
            var m = new Maker(new MiruSolution(_solutionDir));
            var appSettingsStaging = m.Solution.AppDir / "appSettings.Staging.yml";
            Files.Create(appSettingsStaging, appSettingsStaging);
            
            // act
            m.AppSettings("Staging");
            
            // assert
            appSettingsStaging.ShouldContain(appSettingsStaging);
        }
        
        [Test]
        public void Create_destination_from_existent_app_settings_example()
        {
            // arrange
            var m = new Maker(new MiruSolution(_solutionDir));
            Files.Create(m.Solution.AppDir / "appSettings-example.yml", "Example");
            
            // act
            m.AppSettings("Production");
            
            // assert
            (m.Solution.AppDir / "appSettings.Production.yml").ShouldContain("Example");
        }
        
        [Test]
        public void Create_destination_from_template_if_app_settings_does_not_exist()
        {
            // arrange
            var m = new Maker(new MiruSolution(_solutionDir));
            
            // act
            m.AppSettings("CI");
            
            // assert
            (m.Solution.AppDir / "appSettings.CI.yml").ShouldContain("ConnectionString");
        }
    }
}