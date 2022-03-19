using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Miru.PageTesting.Tests;

[SetUpFixture]
public class Program
{
    public static async Task Main(string[] args)
    {
        await new RootCommand().InvokeAsync(args);
    }
    //
    // [OneTimeSetUp]
    // public void Setup() => DriverFixture.Get.Value.ToString();
    //
    [OneTimeTearDown]
    public void Teardown()
    {
        DriverFixture.Get.Value.Dispose();
    }
}