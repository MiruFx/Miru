using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Ardalis.SmartEnum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Miru.Mvc;
using Miru.Pagination;
using Miru.Urls;

namespace Miru.Tests.Urls;

public class UrlTest : MiruCoreTest
{
    private UrlLookup _url;

    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var listener = new DiagnosticListener("Microsoft.AspNetCore");
        
        return services
            .AddMvcCore()
            .AddMiruActionResult()
            .AddControllersAsServices()
            .AddMiruNestedControllerUnder<UrlTest>()
            .Services
            .AddSingleton(listener)
            .AddSingleton<DiagnosticSource>(listener)
            .AddMiruUrls(x =>
            {
                x.Base = "https://mirufx.github.io";
            })
            .AddControllersWithViews()
            .AddApplicationPart(typeof(UrlTest).Assembly)
            .Services
            //
            .AddLogging();
    }

    [OneTimeSetUp]
    public void FixtureSetup()
    {
        // _.Get<UrlMapsScanner>().Scan();
        
        _url = _.Get<UrlLookup>();
    }

    [Test]
    public void Url_for_a_type()
    {
        _url.For<ProductsList.Query>()
            .ShouldBe("/Products/List");
    }
        
    [Test]
    public void Url_for_different_actions_with_same_controller_name()
    {
        _url.For<ProductsList.Query>()
            .ShouldBe("/Products/List");
            
        _url.For(new ProductsEdit.Query { Id = 10 })
            .ShouldBe("/Products/Edit?Id=10");
    }
        
    [Test]
    public void Url_for_an_instance_when_routing_constraints()
    {
        _url.For(new ProductsList.Query { Category = "Clothes" })
            .ShouldBe("/Products/List/Clothes");
    }

    [Test]
    public void Url_for_an_instance_when_routing_constraints_and_query_string()
    {
        _url.For(new ProductsList.Query { Category = "Jeans", MaxPrice = 70 })
            .ShouldBe("/Products/List/Jeans?MaxPrice=70");
    }
        
    [Test]
    public void Url_for_an_instance_when_query_string()
    {
        _url.For(new ProductsList.Query { OnSales = true })
            .ShouldBe("/Products/List?OnSales=True");
    }
        
    [Test]
    public void Can_ignore_items_from_a_list()
    {
        var request = new ProductsList.Query
        {
            Size = new List<ProductsList.Size>
            {
                ProductsList.Size.Small,
                ProductsList.Size.Medium,
                ProductsList.Size.Large
            }
        };
        
        _url.Build(request)
            .Without(m => m.Size, ProductsList.Size.Medium)
            .Without(m => m.Size, ProductsList.Size.Large)
            .ToString()
            .ShouldBe("/Products/List?Size%5B0%5D=Small");
    }
        
    [Test]
    public void Can_ignore_whole_array()
    {
        var request = new ProductsList.Query
        {
            CategoryId = 123,
            Size = new List<ProductsList.Size>
            {
                ProductsList.Size.Small,
                ProductsList.Size.Medium
            }
        };
        
        _url.Build(request).Without(m => m.Size)
            .ToString()
            .ShouldBe("/Products/List?CategoryId=123");
    }
        
    [Test]
    public void Can_ignore_a_property_with_a_value()
    {
        var request = new ProductsList.Query
        {
            CategoryId = 123,
            Category = "Apple"
        };
            
        _url.Build(request)
            .Without(m => m.CategoryId, 123)
            .ToString()
            .ShouldBe("/Products/List/Apple");
                    
        _url.Build(request)
            .Without(m => m.CategoryId, 111)
            .ToString()
            .ShouldBe("/Products/List/Apple?CategoryId=123");
    }
        
    [Test]
    public void Can_add_item_to_array_property()
    {
        var request = new ProductsList.Query
        {
            Size = new List<ProductsList.Size>
            {
                ProductsList.Size.Small,
                ProductsList.Size.Medium
            }
        };
        
        _url.Build(request).With(m => m.Size, ProductsList.Size.Large)
            .ToString()
            .ShouldBe("/Products/List?Size%5B0%5D=Small&Size%5B1%5D=Medium&Size%5B2%5D=Large");
    }
        
    [Test]
    public void Pageable_url()
    {
        _url.For(new ProductsList.Query()
            {
                Category = "Men",
                Page = 2,
                PageSize = 10
            })
            .ShouldBe("/Products/List/Men?Page=2&PageSize=10");
    }
        
    [Test]
    public void When_paging_url_should_ignore_paging_counters()
    {
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
        
        _url.For(request)
            .ShouldBe("/Products/List/Shoes?Page=2&PageSize=100");
    }
    
    [Test]
    public void Should_ignore_select_lookup_properties()
    {
        var request = new PaymentList.Query()
        {
            PayerId = 100
        };
        
        _url.For(request).ShouldBe("/payments?PayerId=100");
    }
    
    [Test]
    public void Should_build_with_only_one_property_ignoring_other()
    {
        var request = new ProductsList.Query
        {
            CategoryId = 123, 
            Category = "Apple",
            OnSales = true
        };
        
        _url.Build(request)
            .WithOnly(m => m.Category, "Samsung")
            .ToString()
            .ShouldBe("/Products/List/Samsung");
            
        // properties' values should remain as before
        request.CategoryId.ShouldBe(123);
        request.Category.ShouldBe("Apple");
        request.OnSales.ShouldBeTrue();
    }
        
    [Test]
    public void When_paging_and_using_modifier_should_erase_paging_counters()
    {
        var request = new ProductsList.Query
        {
            Category = "Men",
                
            Page = 399,
            PageSize = 20,
        };
            
        _url.Build(request)
            .With(m => m.Category, "Women")
            .ToString()
            .ShouldBe("/Products/List/Women");
    }
        
    [Test]
    public void Should_ignore_if_property_type_is_named_result()
    {
        var request = new ProductsList.Query
        {
            Category = "Apple",
            Results = new List<ProductsList.Result>
            {
                new() { ProductName = "Product 1", ProductPrice = 10 },
                new() { ProductName = "Product 2", ProductPrice = 20 }
            }
        };
        
        _url.For(request)
            .ShouldBe("/Products/List/Apple");
    }
        
    [Test]
    public void When_using_modifiers_should_not_change_request_state()
    {
        var request = new ProductsList.Query
        {
            CategoryId = 123, 
            Category = "Apple",
            OnSales = true
        };
        
        _url.Build(request)
            .With(m => m.Category, "Samsung")
            .With(m => m.OnSales, false)
            .ToString().ShouldBe("/Products/List/Samsung?CategoryId=123&OnSales=False");
            
        // properties' values should remain as before
        request.CategoryId.ShouldBe(123);
        request.Category.ShouldBe("Apple");
        request.OnSales.ShouldBeTrue();
    }
        
    [Test]
    public void Should_generate_query_string_when_input_has_null_or_default_property_values()
    {
        _url.Build(new ProductsList.Query())
            .With(x => x.Category, "Men")
            .ToString()
            .ShouldBe("/Products/List/Men");
    }
        
    [Test]
    public void No_query_string()
    {
        _url.For(new ProductsList.Query())
            .ShouldBe("/Products/List");
    }
        
    [Test]
    public void Should_ignore_null_properties()
    {
        var request = new ProductsList.Query
        {
            CategoryId = 123,
            Category = null,
            OnSales = true
        };
        
        _url.For(request)
            .ShouldBe("/Products/List?CategoryId=123&OnSales=True");
    }
        
    [Test]
    public void Bug_should_handle_input_filled_with_simple_property_and_one_override()
    {
        var request = new ProductsList.Query
        {
            CategoryId = 123
        };
        
        _url.Build(request)
            .With(m => m.Category, "Samsung")
            .ToString()
            .ShouldBe("/Products/List/Samsung?CategoryId=123");
    }
             
    [Test]
    public void Bug_should_ignore_default_values()
    {
        var request = new ProductsList.Query
        {
            Category = "Cars",
            Size = new List<ProductsList.Size>
            {
                ProductsList.Size.Small,
                ProductsList.Size.Medium,
                ProductsList.Size.Large
            }
        };
        
        _url.Build(request)
            .With(m => m.Page, 3)
            .Without(m => m.Category)
            .Without(m => m.Size, ProductsList.Size.Medium)
            .Without(m => m.Size, ProductsList.Size.Large)
            .ToString()
            .ShouldBe("/Products/List?Size%5B0%5D=Small&Page=3");
    }
        
    [Test]
    public void Paging_should_ignore_default_values()
    {
        PaginationConfig.DefaultPageSize = 10;
        
        var request = new ProductsList.Query
        {
            Page = 1,
            PageSize = PaginationConfig.DefaultPageSize
        };
        
        _url.Build(request)
            .ToString()
            .ShouldBe("/Products/List");
        
        _url.Build(request)
            .With(m => m.Page, 2)
            .ToString()
            .ShouldBe("/Products/List?Page=2");
        
        _url.Build(request)
            .With(m => m.PageSize, 20)
            .ToString()
            .ShouldBe("/Products/List?PageSize=20");
    }
        
    [Test]
    public void Should_ignore_readonly_properties()
    {
        var request = new ProductsList.Query
        {
            Category = "Food"
        };
        
        request.Title.ShouldBe("Food");
            
        _url.For(request)
            .ShouldBe("/Products/List/Food");
    }
        
    [Test]
    public void Should_ignore_enumerable_properties()
    {
        var request = new ProductsList.Query();
        
        request.Categories.Count().ShouldBe(2);
            
        _url.For(request)
            .ShouldNotContain("Categories");
    }
        
    [Test]
    public void Should_readonly_lists_or_collection_properties()
    {
        var request = new ProductsList.Query();
        
        request.Categories.Count().ShouldBe(2);
            
        var url = _url.For(request);
                
        url.ShouldNotContain(nameof(ProductsList.Query.ReadOnlyCollection));
        url.ShouldNotContain(nameof(ProductsList.Query.ReadOnlyList));
    }
        
    [Test]
    public void Should_ignore_dictionary_properties()
    {
        var request = new ProductsEdit.Command()
        {
            Id = 1,
            ShouldIgnore = true
        };
        
        _url.For(request)
            .ShouldNotContain("ShouldIgnore");
    }
        
    [Test]
    public void Should_ignore_properties_marked_with_url_ignore()
    {
        var request = new OrderList.Query();
    
        request.Categories.Count.ShouldBe(2);
        
        _url.For(request)
            .ShouldNotContain("Categories");
    }
    
    [Test]
    public void Should_throw_exception_if_model_is_not_mapped_to_a_route()
    {
        Should.Throw<MiruException>(() => _url.For<NotMapped>());
    }
            
    [Test]
    public void If_request_is_command_ignore_querystring()
    {
        var request = new ProductsEdit.Command
        {
            Id = 10,
            Name = "iPhone 5S",
            ReturnUrl = "/Products"
        };
    
        _url.For(request)
            .ShouldBe("/Products/Edit/10");
    }
    
    [Test]
    public void Build_full_url()
    {
        var request = new ProductsEdit.Command
        {
            Id = 10,
            Name = "iPhone 5S",
            ReturnUrl = "/Products"
        };
    
        _url.FullFor(request)
            .ShouldBe("https://mirufx.github.io/Products/Edit/10");
    }
    
    [Test]
    public void When_building_full_url_ignore_last_bar()
    {
        // arrange
        // _.Get<UrlOptions>().Base = "https://mirufx.github.io/";
        
        var request = new ProductsEdit.Command
        {
            Id = 10,
            Name = "iPhone 5S",
            ReturnUrl = "/Products"
        };
    
        // act
        var url1 = _url.FullFor(request);
        var url2 = _url.FullFor<ProductsEdit>();
            
        // assert
        url1.ShouldBe("https://mirufx.github.io/Products/Edit/10");
        url2.ShouldBe("https://mirufx.github.io/Products/Edit");
    }
    
    [Test]
    public void Build_query_string_for_date()
    {
        var currentCulture = CultureInfo.CurrentCulture;
    
        CultureInfo.CurrentCulture = new CultureInfo("pt-BR");
        
        var request = new ProductsList.Query
        {
            SoldBefore = new DateTime(2020, 1, 31, 10, 11, 12),
        };
    
        _url.For(request)
            .ShouldBe("/Products/List?SoldBefore=31%2F01%2F2020");
    
        CultureInfo.CurrentCulture = currentCulture;
    }
    
    [Test]
    public void Build_query_string_for_date_only()
    {
        var currentCulture = CultureInfo.CurrentCulture;
    
        CultureInfo.CurrentCulture = new CultureInfo("pt-BR");
        
        var request = new ProductsList.Query
        {
            SoldAfter = new DateOnly(2011, 8, 31),
        };
    
        _url.For(request)
            .ShouldBe("/Products/List?SoldAfter=2011-08-31");
    
        CultureInfo.CurrentCulture = currentCulture;
    }
    
    [Test]
    public void Build_query_string_for_enumeration_of_t()
    {
        var request = new ProductsList.Query
        {
            ProductStatus = ProductsList.ProductStatus.OutOfStock,
        };
    
        _url.For(request)
            .ShouldBe($"/Products/List?ProductStatus={ProductsList.ProductStatus.OutOfStock.Value}");
        
        _url.Build(new ProductsList.Query())
            .With(x => x.ProductStatus, ProductsList.ProductStatus.OutOfStock)
            .ToString()
            .ShouldBe($"/Products/List?ProductStatus={ProductsList.ProductStatus.OutOfStock.Value}");
    }
    
    [Test]
    public void Build_query_string_with_string_property_name_and_for_enumeration_of_from_string()
    {
        _url.Build(new ProductsList.Query())
            .With("ProductStatus", "2")
            .ToString()
            .ShouldBe($"/Products/List?ProductStatus={ProductsList.ProductStatus.OutOfStock.Value}");
    }
   
    [Test]
    public void Build_query_string_for_enumeration_of_t1_and_t2()
    {
        var request = new ProductsList.Query
        {
            OrderStatus = ProductsList.OrderStatus.Delivered,
        };
    
        _url.For(request)
            .ShouldBe($"/Products/List?OrderStatus={ProductsList.OrderStatus.Delivered.Value}");
        
        _url.Build(new ProductsList.Query())
            .With(x => x.OrderStatus, ProductsList.OrderStatus.Paid)
            .ToString()
            .ShouldBe($"/Products/List?OrderStatus={ProductsList.OrderStatus.Paid.Value}");            
    }
    
    [Test]
    public void If_property_has_attribute_from_query_then_should_build_url_with_attribute_name()
    {
        _url.For(new ProductsList.Query { PartnerId = 100 })
            .ShouldBe($"/Products/List?pid=100");
    }

    public class NotMapped
    {
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
        
    public class ProductsList
    {
        public class Query : IPageable<Result>
        {
            public int CategoryId { get; set; }
            public string Category { get; set; }
            public int MaxPrice { get; set; }
            public bool OnSales { get; set; }
            public string Title => Category.IfEmpty("All Categories");
            
            [FromQuery(Name = "pid")]
            public int PartnerId { get; set; }
                
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
            public DateOnly SoldAfter { get; set; }
            public ProductStatus ProductStatus { get; set; }
            public OrderStatus OrderStatus { get; set; }
        }

        public class ProductStatus : SmartEnum<ProductStatus>
        {
            public static ProductStatus Active = new(1, "Active");
            public static ProductStatus OutOfStock = new(2, "Out Of Stock");
                
            public ProductStatus(int value, string name) : base(name, value)
            {
            }
        }
            
        public class OrderStatus : SmartEnum<OrderStatus>
        {
            public static OrderStatus Paid = new(1, "Paid");
            public static OrderStatus Delivered = new(2, "Delivered");
                
            public OrderStatus(int value, string name) : base(name, value)
            {
            }
        }

        public enum Size
        {
            Small = 1,
            Medium = 2,
            Large = 3
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

    public class PaymentList
    {
        public class Query
        {
            public long PayerId { get; set; }
            public SelectLookups States { get; set; } = new[] { "MG", "MT", "DF" }.ToSelectLookups();
        }
        
        public class PaymentController : MiruController
        {
            [HttpGet("/payments")]
            public Query Edit(Query request) => request;
        }
    }
}