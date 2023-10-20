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
    
    public class Yml
    {
        [SetUp]
        public void Setup()
        {
            YmlConfig.PropertyFilters.Add(p => p.Name != "CategoryId");
        }
        
        [Test]
        public void Can_convert_object_to_yaml_string()
        {
            var model = new ProductList.Query
            {
                CategoryId = 123,
                Category = "Apple",
                OnSales = true,
                Size = new List<ProductList.Size>
                {
                    ProductList.Size.Small,
                    ProductList.Size.Medium
                },
                Result = new ProductList.Result
                {
                    Total = 100
                }
            };

            model.ToYml().DumpToConsole();
        }
            
        [Test]
        [Ignore("Will not be used now. Filter password on DumpBehavior")]
        public void Can_filter_properties_will_be_converted()
        {
            var model = new ProductList.Query
            {
                
                CategoryId = 123,
                OnSales = true,
                Result = new ProductList.Result
                {
                    Total = 100
                }
            };

            var yaml = model.ToYml().DumpToConsole();
        }
            
        [Test]
        public void Can_ignore_properties()
        {
            var model = new ProductList.Query
            {
                CategoryId = 123,
            };

            model.ToYml().DumpToConsole().ShouldNotContain("CategoryId");
        }
    }
}