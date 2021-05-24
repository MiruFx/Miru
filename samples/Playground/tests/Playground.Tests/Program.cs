using Miru.Testing;

namespace Playground.Tests
{
    public class Program
    {
        public static int Main(string[] args) => new TestRunner().RunAssemblyOfType<Program>(args);
    }
}
