using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Miru.Core;
using Miru.Domain;
using Miru.Foundation.Hosting;
using Miru.Mvc;
using Miru.Pagination;
using Miru.Urls;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Urls
{
    public class UrlTest : IDisposable
    {
        private readonly MiruTestWebHost _host = new MiruTestWebHost(MiruHost.CreateMiruHost(), 
            services =>
            {
                services
                    .AddMvcCore()
                    .AddMiruActionResult()
                    .AddMiruNestedControllers();
                    
                services.AddMiruUrls(x =>
                {
                    x.Base = "https://mirufx.github.io";
                });
                
                services.AddControllersWithViews();
            });
        
        private UrlLookup UrlLookup => _host.Services.GetService<UrlLookup>();

        public void Dispose() => _host?.Dispose();
        
        [Test]
        public void Url_for_a_type()
        {
            UrlLookup
                .For<ProductsList.Query>()
                .ShouldBe("/Products/List");
        }

        [Test]
        public void Url_for_different_actions_with_same_controller_name()
        {
            UrlLookup
                .For<ProductsList.Query>()
                .ShouldBe("/Products/List");
            
            UrlLookup
                .For(new ProductsEdit.Query { Id = 10 })
                .ShouldBe("/Products/Edit?Id=10");
        }
        
        [Test]
        public void Url_for_an_instance_when_routing_constraints()
        {
            UrlLookup
                .For(new ProductsList.Query { Category = "Clothes" })
                .ShouldBe("/Products/List/Clothes");
        }
        
        [Test]
        public void Url_for_an_instance_when_routing_constraints_and_query_string()
        {
            UrlLookup
                .For(new ProductsList.Query { Category = "Jeans", MaxPrice = 70 })
                .ShouldBe("/Products/List/Jeans?MaxPrice=70");
        }
        
        [Test]
        public void Url_for_an_instance_when_query_string()
        {
            UrlLookup
                .For(new ProductsList.Query { OnSales = true })
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
        
            UrlLookup
                .Build(request)
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
        
            UrlLookup
                .Build(request).Without(m => m.Size)
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
            
            UrlLookup
                .Build(request)
                .Without(m => m.CategoryId, 123)
                .ToString()
                .ShouldBe("/Products/List/Apple");
                    
            UrlLookup
                .Build(request)
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
        
            UrlLookup
                .Build(request).With(m => m.Size, ProductsList.Size.Large)
                .ToString()
                .ShouldBe("/Products/List?Size%5B0%5D=Small&Size%5B1%5D=Medium&Size%5B2%5D=Large");
        }
        
        [Test]
        public void Pageable_url()
        {
            UrlLookup
                .For(new ProductsList.Query()
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

            UrlLookup
                .For(request)
                .ShouldBe("/Products/List/Shoes?Page=2&PageSize=100");
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
            
            UrlLookup
                .Build(request)
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
                    new ProductsList.Result() { ProductName = "Product 1", ProductPrice = 10 },
                    new ProductsList.Result() { ProductName = "Product 2", ProductPrice = 20 }
                }
            };

            UrlLookup
                .For(request)
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

            UrlLookup
                .Build(request)
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
            UrlLookup
                .Build(new ProductsList.Query())
                .With(x => x.Category, "Men")
                .ToString()
                .ShouldBe("/Products/List/Men");
        }
        
        [Test]
        public void No_query_string()
        {
            UrlLookup
                .For(new ProductsList.Query())
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

            UrlLookup
                .For(request)
                .ShouldBe("/Products/List?CategoryId=123&OnSales=True");
        }

        [Test]
        public void Bug_should_handle_input_filled_with_simple_property_and_one_override()
        {
            var request = new ProductsList.Query
            {
                CategoryId = 123
            };

            UrlLookup
                .Build(request)
                .With(m => m.Category, "Samsung")
                .ToString()
                .ShouldBe("/Products/List/Samsung?CategoryId=123");
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

            UrlLookup
                .Build(request)
                .ToString()
                .ShouldBe("/Products/List");

            UrlLookup
                .Build(request)
                .With(m => m.Page, 2)
                .ToString()
                .ShouldBe("/Products/List?Page=2");

            UrlLookup
                .Build(request)
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
            
            UrlLookup
                .For(request)
                .ShouldBe("/Products/List/Food");
        }

        [Test]
        public void Should_ignore_enumerable_properties()
        {
            var request = new ProductsList.Query();

            request.Categories.Count().ShouldBe(2);
            
            UrlLookup
                .For(request)
                .ShouldNotContain("Categories");
        }
        
        [Test]
        public void Should_readonly_lists_or_collection_properties()
        {
            var request = new ProductsList.Query();

            request.Categories.Count().ShouldBe(2);
            
            var url = UrlLookup.For(request);
                
            url.ShouldNotContain(nameof(ProductsList.Query.ReadOnlyCollection));
            url.ShouldNotContain(nameof(ProductsList.Query.ReadOnlyList));
        }
        
        [Test]
        public void Should_ignore_dictionary_properties()
        {
            var request = new OrderList.Query();

            request.Categories.Count.ShouldBe(2);
            
            UrlLookup
                .For(request)
                .ShouldNotContain("Categories");
        }

        [Test]
        public void Should_throw_exception_if_model_is_not_mapped_to_a_route()
        {
            Should.Throw<MiruException>(() => UrlLookup.For<NotMapped>());
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
        
            UrlLookup
                .For(request)
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
        
            UrlLookup
                .FullFor(request)
                .ShouldBe("https://mirufx.github.io/Products/Edit/10");
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
        
            UrlLookup
                .For(request)
                .ShouldBe("/Products/List?SoldBefore=31%2F01%2F2020");

            CultureInfo.CurrentCulture = currentCulture;
        }
        
        [Test]
        public void Build_query_string_for_enumeration_of_t()
        {
            var request = new ProductsList.Query
            {
                ProductStatus = ProductsList.ProductStatus.OutOfStock,
            };
        
            UrlLookup
                .For(request)
                .ShouldBe($"/Products/List?ProductStatus={ProductsList.ProductStatus.OutOfStock.Value}");
            
            UrlLookup
                .Build(new ProductsList.Query())
                .With(x => x.ProductStatus, ProductsList.ProductStatus.OutOfStock)
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
        
            UrlLookup
                .For(request)
                .ShouldBe($"/Products/List?OrderStatus={ProductsList.OrderStatus.Delivered.Value}");
            
            UrlLookup
                .Build(new ProductsList.Query())
                .With(x => x.OrderStatus, ProductsList.OrderStatus.Paid)
                .ToString()
                .ShouldBe($"/Products/List?OrderStatus={ProductsList.OrderStatus.Paid.Value}");            
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
                public string Title => Category.Or("All Categories");
                
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

            public class ProductStatus : Enumeration<ProductStatus, string>
            {
                public static ProductStatus Active = new("A", "Active");
                public static ProductStatus OutOfStock = new("O", "Out Of Stock");
                
                public ProductStatus(string value, string name) : base(value, name)
                {
                }
            }
            
            public class OrderStatus : Enumeration<OrderStatus>
            {
                public static OrderStatus Paid = new(1, "Paid");
                public static OrderStatus Delivered = new(2, "Delivered");
                
                public OrderStatus(int value, string name) : base(value, name)
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
            }

            public class ProductsController : MiruController
            {
                public Command Edit(Query request) => new Command();
                
                [HttpPost, Route("/Products/Edit/{Id}")]
                public Result Edit(Command request) => new Result();
            }
        }
    }
}