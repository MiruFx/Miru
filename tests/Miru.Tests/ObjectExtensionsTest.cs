namespace Miru.Tests;

public class ObjectExtensionsTest
{
    [Test]
    public void Should_convert_object_to_int()
    {
        25.00m.ToInt().ShouldBe(25);
        "25".ToInt().ShouldBe(25);
    }
}