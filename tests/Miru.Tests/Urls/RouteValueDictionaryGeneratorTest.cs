using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Miru.Domain;
using Miru.Mvc;
using Miru.Pagination;
using Miru.Urls;

namespace Miru.Tests.Urls;

public class RouteValueDictionaryGeneratorTest
{
    private RouteValueDictionaryGenerator _generator;

    [OneTimeSetUp]
    public void Setup()
    {
        _generator = new RouteValueDictionaryGenerator(new UrlOptions());
    }
    
    [Test]
    public void For_empty_request()
    {
        var dic = _generator.Generate(new ProductList.Query());
        
        dic.ShouldCount(0);
    }
        
    [Test]
    public void For_request_with_int_property()
    {
        var dic = _generator.Generate(new ProductsEdit.Query
        {
            Id = 10
        });
        
        dic.ShouldCount(1);
        dic["Id"].ShouldBe("10");
    }
        
    [Test]
    public void For_request_with_string_property()
    {
        var dic = _generator.Generate(new ProductsList.Query
        {
            Category = "Clothes"
        });
        
        dic.ShouldCount(1);
        dic["Category"].ShouldBe("Clothes");
    }
    
    [Test]
    public void For_request_with_many_properties()
    {
        var dic = _generator.Generate(new ProductsList.Query
        {
            Category = "Jeans",
            MaxPrice = 70,
            OnSales = true
        });
        
        dic.ShouldCount(3);
        dic["Category"].ShouldBe("Jeans");
        dic["MaxPrice"].ShouldBe("70");
        dic["OnSales"].ShouldBe("True");
    }

    [Test]
    public void Can_ignore_items_from_a_list()
    {
        var modifiers = new UrlBuilderModifiers();
        
        modifiers.WithoutValues.Add("Size", Size.Medium);
        modifiers.WithoutValues.Add("Size", Size.Large);
        
        var dic = _generator.Generate(new ProductsList.Query
        {
            Size = new List<Size>
            {
                Size.Small,
                Size.Medium,
                Size.Large
            }
        }, modifiers);
        
        dic.ShouldCount(1);
        dic["Size[0]"].ShouldBe(Size.Small);
    }
        
    [Test]
    public void Can_ignore_whole_array()
    {
        // arrange
        var request = new ProductsList.Query
        {
            CategoryId = 123,
            Size = new List<Size>
            {
                Size.Small,
                Size.Medium
            }
        };
        
        var modifiers = new UrlBuilderModifiers();
        
        modifiers.Without.Add("Size");
        
        // act
        var dic = _generator.Generate(request, modifiers);
        
        // assert
        dic.ShouldCount(1);
        dic["CategoryId"].ShouldBe("123");
    }
        
    [Test]
    public void Can_ignore_a_property_with_a_value()
    {
        // arrange
        var request = new ProductsList.Query
        {
            CategoryId = 123,
            Category = "Apple"
        };
        
        var modifiers = new UrlBuilderModifiers();
        modifiers.WithoutValues.Add("CategoryId", 123);
        
        // act
        var dic = _generator.Generate(request, modifiers);
        
        // assert
        dic.ShouldCount(1);
    }
    
    [Test]
    public void Should_not_ignore_a_property_with_different_value_than_modifier()
    {
        // arrange
        var request = new ProductsList.Query
        {
            CategoryId = 123,
            Category = "Apple"
        };
        
        var modifiers = new UrlBuilderModifiers();
        modifiers.WithoutValues.Add("CategoryId", 111);
        
        // act
        var dic = _generator.Generate(request, modifiers);
        
        // assert
        dic.ShouldCount(2);
        dic["Category"].ShouldBe("Apple");
        dic["CategoryId"].ShouldBe("123");
    }
        
    [Test]
    public void If_with_modifier_when_property_is_list_then_should_append_value()
    {
        // arrange
        var request = new ProductsList.Query
        {
            Size = new List<Size>
            {
                Size.Small,
                Size.Medium
            }
        };
        
        var modifiers = new UrlBuilderModifiers();
        modifiers.With.Add("Size", Size.Large);
        
        // act
        var dic = _generator.Generate(request, modifiers);
        
        // assert
        dic.ShouldCount(3);
        dic["Size[0]"].ShouldBe(Size.Small);
        dic["Size[1]"].ShouldBe(Size.Medium);
        dic["Size[2]"].ShouldBe(Size.Large);
    }
        
    [Test]
    public void Should_add_pageable_properties()
    {
        // arrange
        var request = new ProductsList.Query()
        {
            Category = "Men",
            Page = 2,
            PageSize = 10
        };
        
        // act
        var dic = _generator.Generate(request);
        
        // assert
        dic.ShouldCount(3);
        dic["Page"].ShouldBe("2");
        dic["PageSize"].ShouldBe("10");
        dic["Category"].ShouldBe("Men");
    }
        
    [Test]
    public void By_default_should_ignore_paging_counters()
    {
        // arrange
        var request = new ProductsList.Query()
        {
            Category = "Shoes",
            Page = 2,
            PageSize = 100,
                
            // counters:
            Pages = 5,
            CountShowing = 20,
            CountTotal = 100
        };
        
        // act
        var dic = _generator.Generate(request);
        
        // assert
        dic.ShouldCount(3);
        dic["Page"].ShouldBe("2");
        dic["PageSize"].ShouldBe("100");
        dic["Category"].ShouldBe("Shoes");
    }
        
    [Test]
    public void When_paging_and_using_modifier_should_erase_paging_counters()
    {
        // arrange
        var request = new ProductsList.Query
        {
            Category = "Men",
                
            Page = 399,
            PageSize = 20,
        };
        
        var modifiers = new UrlBuilderModifiers();
        modifiers.With.Add("Category", "Women");
        
        // act
        var dic = _generator.Generate(request, modifiers);
        
        // assert
        dic.ShouldCount(1);
        dic["Category"].ShouldBe("Women");
    }
    
    // TODO: should move to a test class that tests default UrlConfig
    [Test]
    public void Should_ignore_if_property_type_is_named_result()
    {
        // arrange
        var request =  new ProductsList.Query
        {
            Category = "Apple",
            Results = new List<ProductsList.Result>
            {
                new() { ProductName = "Product 1", ProductPrice = 10 },
                new() { ProductName = "Product 2", ProductPrice = 20 }
            }
        };
        
        // act
        var dic = _generator.Generate(request);
        
        // assert
        dic.ShouldCount(1);
        dic["Category"].ShouldBe("Apple");
    }
    
    [Test]
    public void When_using_modifiers_should_not_change_request_state()
    {
        // arrange
        var request = new ProductsList.Query
        {
            CategoryId = 123, 
            Category = "Apple",
            OnSales = true
        };
        
        var modifiers = new UrlBuilderModifiers();
        modifiers.With.Add("Category", "Samsung");
        modifiers.With.Add("OnSales", false);
        
        // act
        var dic = _generator.Generate(request, modifiers);
        
        dic.ShouldCount(3);
        dic["Category"].ShouldBe("Samsung");
        dic["OnSales"].ShouldBe("False");
        dic["CategoryId"].ShouldBe("123");
    }
        
    [Test]
    public void Should_generate_query_string_when_input_has_null_or_default_property_values()
    {
        // arrange
        var request = new ProductsList.Query();
        
        var modifiers = new UrlBuilderModifiers();
        modifiers.With.Add("Category", "Men");
        
        // act
        var dic = _generator.Generate(request, modifiers);
        
        // assert
        dic.ShouldCount(1);
        dic["Category"].ShouldBe("Men");
    }
        
    [Test]
    public void No_query_string()
    {
        // arrange
        var request = new ProductsList.Query();
        
        // act
        var dic = _generator.Generate(request);
        
        // assert
        dic.ShouldCount(0);
    }
        
    [Test]
    public void Should_ignore_null_properties()
    {
        // arrange
        var request = new ProductsList.Query
        {
            CategoryId = 123,
            Category = null,
            OnSales = true
        };
        
        // act
        var dic = _generator.Generate(request);
        
        // assert
        dic.ShouldCount(2);
        dic["CategoryId"].ShouldBe("123");
        dic["OnSales"].ShouldBe("True");
    }
        
    [Test]
    public void Bug_should_handle_input_filled_with_simple_property_and_one_override()
    {
        // arrange
        var request = new ProductsList.Query
        {
            CategoryId = 123
        };
        
        var modifiers = new UrlBuilderModifiers();
        modifiers.With.Add("Category", "Samsung");
        
        // act
        var dic = _generator.Generate(request, modifiers);
        
        // assert
        dic.ShouldCount(2);
        dic["Category"].ShouldBe("Samsung");
        dic["CategoryId"].ShouldBe("123");
    }
             
    [Test]
    public void Bug_should_ignore_default_values()
    {
        // arrange
        var request = new ProductsList.Query
        {
            Category = "Cars",
            Size = new List<Size>
            {
                Size.Small,
                Size.Medium,
                Size.Large
            }
        };
        
        var modifiers = new UrlBuilderModifiers();
        modifiers.With.Add("Page", 3);
        modifiers.Without.Add("Category");
        modifiers.WithoutValues.Add("Size", Size.Medium);
        modifiers.WithoutValues.Add("Size", Size.Large);
        
        // act
        var dic = _generator.Generate(request, modifiers);
        
        // assert
        dic.ShouldCount(2);
        dic["Size[0]"].ShouldBe(Size.Small);
        dic["Page"].ShouldBe(3);
    }
    
    // [Test]
    // public void Paging_should_ignore_default_values()
    // {
    //     PaginationConfig.DefaultPageSize = 10;
    //     
    //     var request = new ProductsList.Query
    //     {
    //         Page = 1,
    //         PageSize = PaginationConfig.DefaultPageSize
    //     };
    //     
    //     _url.Build(request)
    //         .ToString()
    //         .ShouldBe("/Products/List");
    //     
    //     _url.Build(request)
    //         .With(m => m.Page, 2)
    //         .ToString()
    //         .ShouldBe("/Products/List?Page=2");
    //     
    //     _url.Build(request)
    //         .With(m => m.PageSize, 20)
    //         .ToString()
    //         .ShouldBe("/Products/List?PageSize=20");
    // }
        
    [Test]
    public void Should_ignore_readonly_properties()
    {
        // arrange
        var request = new ProductsList.Query
        {
            Category = "Food"
        };
        
        // act
        var dic = _generator.Generate(request);
        
        // assert
        request.TitleReadOnly.ShouldBe("Food");

        dic.ShouldCount(1);
        dic["Category"].ShouldBe("Food");
    }
    
    [Test]
    public void Should_ignore_enumerable_properties()
    {
        // arrange
        var request = new ProductsList.Query();

        // act
        var dic = _generator.Generate(request);
        
        // assert
        request.Categories.Count().ShouldBe(2);

        dic["Categories"].ShouldBeNull();
    }
    
    [Test]
    public void Should_readonly_lists_or_collection_properties()
    {
        // arrange
        var request = new ProductsList.Query();

        // act
        var dic = _generator.Generate(request);
        
        // assert
        request.Categories.Count().ShouldBe(2);

        dic["ReadOnlyCollection"].ShouldBeNull();
        dic["ReadOnlyList"].ShouldBeNull();
    }
    
    [Test]
    public void Should_ignore_dictionary_properties()
    {
        // arrange
        var request = new ProductsEdit.Command()
        {
            Id = 1,
            ShouldIgnore = true
        };

        // act
        var dic = _generator.Generate(request);
        
        // assert
        dic["ShouldIgnore"].ShouldBeNull();
    }
    
    [Test]
    public void Should_ignore_properties_marked_with_url_ignore()
    {
        // arrange
        var request = new OrderList.Query();

        // act
        var dic = _generator.Generate(request);
        
        // assert
        request.Categories.Count.ShouldBe(2);

        dic["Categories"].ShouldBeNull();
    }
    
    // TODO: move to another class
    // [Test]
    // public void Should_throw_exception_if_model_is_not_mapped_to_a_route()
    // {
    //     Should.Throw<MiruException>(() => _url.For<NotMapped>());
    // }
    //        
    // TODO: move to another class
    // [Test]
    // public void If_request_is_command_ignore_querystring()
    // {
    //     var request = new ProductsEdit.Command
    //     {
    //         Id = 10,
    //         Name = "iPhone 5S",
    //         ReturnUrl = "/Products"
    //     };
    //
    //     _url.For(request)
    //         .ShouldBe("/Products/Edit/10");
    // }
    //
    // TODO: move to another class
    // [Test]
    // public void Build_full_url()
    // {
    //     var request = new ProductsEdit.Command
    //     {
    //         Id = 10,
    //         Name = "iPhone 5S",
    //         ReturnUrl = "/Products"
    //     };
    //
    //     _url.FullFor(request)
    //         .ShouldBe("https://mirufx.github.io/Products/Edit/10");
    // }
    //
    // TODO: move to another class
    // [Test]
    // public void When_building_full_url_ignore_last_bar()
    // {
    //     // arrange
    //     _host.Services.GetService<UrlOptions>().Base = "https://mirufx.github.io/";
    //     
    //     var request = new ProductsEdit.Command
    //     {
    //         Id = 10,
    //         Name = "iPhone 5S",
    //         ReturnUrl = "/Products"
    //     };
    //
    //     // act
    //     var url1 = _url.FullFor(request);
    //     var url2 = _url.FullFor<ProductsEdit>();
    //         
    //     // assert
    //     url1.ShouldBe("https://mirufx.github.io/Products/Edit/10");
    //     url2.ShouldBe("https://mirufx.github.io/Products/Edit");
    // }
    //
    // TODO: move to another class
    // [Test]
    // public void Build_query_string_for_date()
    // {
    //     var currentCulture = CultureInfo.CurrentCulture;
    //
    //     CultureInfo.CurrentCulture = new CultureInfo("pt-BR");
    //     
    //     var request = new ProductsList.Query
    //     {
    //         SoldBefore = new DateTime(2020, 1, 31, 10, 11, 12),
    //     };
    //
    //     _url.For(request)
    //         .ShouldBe("/Products/List?SoldBefore=31%2F01%2F2020");
    //
    //     CultureInfo.CurrentCulture = currentCulture;
    // }
    //
    
    [Test]
    public void Build_query_string_for_enumeration_of_t()
    {
        // arrange
        var request = new ProductsList.Query
        {
            ProductStatus = ProductStatus.OutOfStock,
        };
        
        var modifiers = new UrlBuilderModifiers();
        modifiers.With.Add("ProductStatus", ProductStatus.OutOfStock);
        
        // act
        var dic = _generator.Generate(request, modifiers);
        
        // assert
        dic.ShouldCount(1);
        dic["ProductStatus"].ShouldBe(ProductStatus.OutOfStock.Value);
    }
    
    // [Test]
    // public void Throw_exception_when_using_enumeration_value_in_modifiers()
    // {
    //     // arrange
    //     var request = new ProductsList.Query
    //     {
    //         ProductStatus = ProductStatus.OutOfStock,
    //     };
    //     
    //     var modifiers = new UrlBuilderModifiers();
    //     modifiers.With.Add("ProductStatus", ProductStatus.OutOfStock.Value);
    //     
    //     // act
    //     Should.Throw<UrlBuilderException>(() => _generator.Generate(request, modifiers));
    // }
    
    [Test]
    public void Build_query_string_for_enumeration_of_t1_and_t2()
    {
        // arrange
        var request = new ProductsList.Query
        {
            OrderStatus = ProductsList.OrderStatus.Delivered,
        };
        
        var modifiers = new UrlBuilderModifiers();
        modifiers.With.Add("OrderStatus", ProductsList.OrderStatus.Paid);
        
        // act
        var dic = _generator.Generate(request, modifiers);
        
        // assert
        dic.ShouldCount(1);
        dic["OrderStatus"].ShouldBe(ProductsList.OrderStatus.Paid.Value.ToString());
    }

    public class OrderList
    {
        public class Query
        {
            public IDictionary<string, string> Categories { get; set; } = new Dictionary<string, string>
            {
                { "M", "Mobile" },
                { "L", "Laptop"}
            };
        }

        public class OrdersController
        {
            public JsonResult List(Query query) => new JsonResult(query);
        }
    }
    
    public class ProductStatus : Enumeration<ProductStatus, string>
    {
        public static ProductStatus Active = new("A", "Active");
        public static ProductStatus OutOfStock = new("O", "Out Of Stock");
                
        public ProductStatus(string value, string name) : base(value, name)
        {
        }
    }
    
    public enum Size
    {
        Small = 1,
        Medium = 2,
        Large = 3
    }

    public class ProductsList
    {
        public class Query : IPageable<Result>
        {
            public int CategoryId { get; set; }
            public string Category { get; set; }
            public int MaxPrice { get; set; }
            public bool OnSales { get; set; }
            public string TitleReadOnly => Category.Or("All Categories");
                
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int Pages { get; set; }
            public int CountShowing { get; set; }
            public int CountTotal { get; set; }
                
            public IReadOnlyList<Result> Results { get; set; }
            public IReadOnlyList<string> ReadOnlyList { get; set; } = new List<string>() { "One", "Two" };
            public IReadOnlyCollection<string> ReadOnlyCollection { get; set; } = new List<string>() { "One", "Two" };
                
            public List<Size> Size { get; set; } = new List<Size>();
                
            public IEnumerable<string> Categories { get; set; } = new List<string>() {"Phone", "Laptop"};
            public DateTime SoldBefore { get; set; }
            public ProductStatus ProductStatus { get; set; }
            public OrderStatus OrderStatus { get; set; }
        }
        
        public class OrderStatus : Enumeration<OrderStatus>
        {
            public static OrderStatus Paid = new(1, "Paid");
            public static OrderStatus Delivered = new(2, "Delivered");
                
            public OrderStatus(int value, string name) : base(value, name)
            {
            }
        }

        public class Result
        {
            public string ProductName { get; set; }
            public decimal ProductPrice { get; set; }
        }
            
        public class ProductsController
        {
            [Route("/Products/List/{Category?}")]
            public JsonResult List(Query query) => new JsonResult(query);
        }
    }

    public class ProductsEdit
    {
        public class Query
        {
            public long Id { get; set; }
        }

        public class Result
        {
        }

        public class Command
        {
            public string ReturnUrl { get; set; }
            public string Name { get; set; }
            public long Id { get; set; }
                
            [UrlIgnore]
            public bool ShouldIgnore { get; set; }
        }

        public class ProductsController : MiruController
        {
            [HttpGet("/Products/Edit")]
            public Command Edit(Query request) => new Command();
                
            [HttpPost("/Products/Edit/{Id}")]
            public Result Edit(Command request) => new Result();
        }
    }
}