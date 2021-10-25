global using System.Linq;
global using System.Threading.Tasks;
global using Corpo.Skeleton.Domain;
global using Corpo.Skeleton.Features.Teams;
global using Miru.Testing;
global using NUnit.Framework;
global using Shouldly;

namespace Corpo.Skeleton.Tests;

public class Program
{
    public static int Main(string[] args) => 
        new TestRunner().RunAssemblyOfType<Program>(args);
}