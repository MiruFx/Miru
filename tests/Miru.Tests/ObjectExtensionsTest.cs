using System.Collections.Generic;

namespace Miru.Tests;

public class ObjectExtensionsTest
{
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