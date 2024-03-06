using Baseline;

namespace Miru.Testing;

public class TestConfigFinder
{
    public static ITestConfig Find(Assembly assembly)
    {
        var testConfigs = assembly
            .ExportedTypes
            .Where(t => t.IsConcrete() && t.Implements<ITestConfig>());

        if (testConfigs.Count() == 1)
            return Activator.CreateInstance(testConfigs.First()) as ITestConfig;
            
        if (testConfigs.None())
            throw new MiruTestConfigException("Could not find a TestConfig class that inherits ITestConfig. Miru needs only one ITestConfig class per test project");
        
        throw new MiruTestConfigException("More than one class inheriting ITestConfig was found. Miru needs only one ITestConfig class per test project");
    }
}