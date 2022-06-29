using System;
using Alba;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Miru.Urls;

namespace Miru.Tests.Urls
{
    public class MiruControllerRoutingDiscoverTest : IDisposable
    {
        private readonly AlbaHost _system = new AlbaHost(
            new MiruTestWebHost(MiruHost.CreateMiruHost()).GetConfiguredHostBuilder());
            
        private UrlLookup UrlLookup => _system.Services.GetService<UrlLookup>();

        public void Dispose() => _system?.Dispose();

        [Test]
        public void Should_map_a_get_action_from_customer_edit()
        {
            var url = UrlLookup.For(new CustomerEdit.Query { Id = 99 });
                
            url.ShouldBe("/Customer/Edit?Id=99");

            _system.Scenario(_ =>
            {
                _.Get.Url(url);
                _.StatusCodeShouldBeOk();
            });
        }
        
        [Test]
        public void Should_map_a_post_action_from_customer_edit()
        {
            var url = UrlLookup.For<CustomerEdit>();
                
            url.ShouldBe("/Customer/Edit");

            _system.Scenario(_ =>
            {
                _.Post.Url(url);
                _.StatusCodeShouldBeOk();
            });
        }
        
        [Test]
        public void Should_map_a_get_action_from_customer_new()
        {
            var url = UrlLookup.For(new CustomerNew());
                
            url.ShouldBe("/Customer/New");

            _system.Scenario(_ =>
            {
                _.Get.Url(url);
                _.StatusCodeShouldBeOk();
            });
        }
        
        [Test]
        public void Should_map_a_post_action_from_customer_new()
        {
            var url = UrlLookup.For<CustomerNew>();
                
            url.ShouldBe("/Customer/New");

            _system.Scenario(_ =>
            {
                _.Post.Url(url);
                _.StatusCodeShouldBeOk();
            });
        }
        
        [Test]
        public void Should_map_index_action()
        {
            var url = UrlLookup.For<CustomerIndex>();
                
            url.ShouldBe("/Customer");

            _system.Scenario(_ =>
            {
                _.Get.Url(url);
                _.StatusCodeShouldBeOk();
            });
        }

        public class CustomerIndex
        {
            public class Query
            {
            }

            public class Result
            {
            }

            public class SomeSimpleQuery
            {
            }
            
            public class CustomerController
            {
                public Result Index(Query request) => new Result();
                
                public Result SomeSimple(SomeSimpleQuery request) => new Result();
            }
        }
        
        public class CustomerEdit
        {
            public class Query
            {
                public long Id { get; set; }
            }

            public class Command
            {
                public long Id { get; set; }
            }
            
            public class Result
            {
                public long Id { get; set; }
            }

            public class CustomerController
            {
                public Command Edit(Query request) => new Command { Id = request.Id };
                
                [HttpPost]
                public Result Edit(Command request) => new Result { Id = request.Id };
            }
        }

        public class CustomerNew
        {
            public class Query
            {
            }
            
            public class Command
            {
            }

            public class Result
            {
            }
            
            public class CustomerController
            {
                public Command New(Query request) => new Command();
                
                [HttpPost]
                public Result New(Command request) => new Result();
            }
        }
    }
}