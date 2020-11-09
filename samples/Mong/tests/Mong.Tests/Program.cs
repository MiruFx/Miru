using Miru.Testing;

namespace Mong.Tests
{
    public class Program
    {
        public static int Main(string[] args) => new TestRunner().RunAssemblyOfType<Program>(args);
    }
}