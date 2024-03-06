using System.Collections.Generic;

namespace Miru.Tests;

public class EnumerableTest
{
    [Test]
    public void Should_return_indexed_enumerable()
    {
        var list1 = new[] { "a", "b", "c" }.Indexed();
        list1.First().index.ShouldBe(0);
        list1.First().item.ShouldBe("a");
        list1.Third().index.ShouldBe(2);
        list1.Third().item.ShouldBe("c");
        
        var list2 = new[] { "d", "e" }.Indexed(startAt: 1);
        list2.First().index.ShouldBe(1);
        list2.First().item.ShouldBe("d");
        list2.Second().index.ShouldBe(2);
        list2.Second().item.ShouldBe("e");
    }
    
    [Test]
    public void Should_convert_object_to_int()
    {
        25.00m.ToInt().ShouldBe(25);
        "25".ToInt().ShouldBe(25);
    }
}