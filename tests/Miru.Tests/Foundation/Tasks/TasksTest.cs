using Microsoft.Extensions.DependencyInjection;
using Miru.Fabrication;
using Miru.Tests.Fabrication;
using NUnit.Framework;

namespace Miru.Tests.Foundation.Tasks
{
    // public class TasksTest
    // {
    //     private ServiceProvider _sp;
    //
    //     [OneTimeSetUp]
    //     public void OneTimeSetup()
    //     {
    //         var services = new ServiceCollection()
    //             .AddCliCommand<Task1>();
    //             
    //         _sp = services.BuildServiceProvider();
    //     }
    //
    //     [Test]
    //     public void Should_run_task1()
    //     {
    //         var rootCommand = new RootCommand
    //         {
    //             new ShowProjectTask("app", m => m.AppDir),
    //             new ShowProjectTask("tests", m => m.AppTestsDir),
    //             new ShowProjectTask("pagetests", m => m.AppPageTestsDir),
    //             new SetupTask("setup"),
    //             new NewTask("new")
    //         };
    //
    //         rootCommand.Name = "miru";
    //         
    //         await rootCommand.InvokeAsync(args);
    //     }
    //
    //     [Test]
    //     public void Should_run_task2()
    //     {
    //     }
    //     
    //     public class Task1 : ICliCommand
    //     {
    //     }
    // }
}