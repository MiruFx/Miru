using Miru.Core;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Makers
{
    public class MakerTest
    {
        [Test]
        public void Can_create_a_maker()
        {
            var m = Maker.For(A.TempPath("Projects"), "Mong");
            m.Solution.Name.ShouldBe("Mong");
            m.Solution.RootDir.ShouldBe(A.TempPath("Projects", "Mong"));
            
            var m2 = Maker.For(A.TempPath("Mong"), "Mong");
            m2.Solution.Name.ShouldBe("Mong");
            m2.Solution.RootDir.ShouldBe(A.TempPath("Mong"));
        }
    }
}