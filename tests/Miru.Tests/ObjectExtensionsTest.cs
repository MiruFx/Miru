using System.Collections.Generic;
using Miru.Core;
using NUnit.Framework;
// using Shoppers.Domain;
using Shouldly;

namespace Miru.Tests;

public class ObjectExtensionsTest
{
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
        
    public class Or
    {
        [Test]
        public void If_object_is_not_null_should_return_its_value()
        {
            new Customer { Name = "Frank" }.Name.Or("No Name").ShouldBe("Frank");
                
            new Customer { Rank = 10 }.Rank.Or("No Rank").ShouldBe("10");
        }
            
        [Test]
        public void If_object_is_null_should_return_or_value()
        {
            new Customer().Name.Or("No Name").ShouldBe("No Name");
                
            new Customer().Rank.Or("No Rank").ShouldBe("No Rank");
        }
            
        [Test]
        public void If_object_is_string_and_empty_should_return_or_value()
        {
            "".Or("No Name").ShouldBe("No Name");
            ((string) null).Or("No Name").ShouldBe("No Name");
        }

        public class Customer
        {
            public string Name { get; set; }
            public long? Rank { get; set; }
            
            public int Ignore { get; set; }
        }
    }
}