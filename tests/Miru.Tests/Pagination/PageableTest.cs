using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutoFixture;
using Miru.Pagination;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Pagination
{
    public class PageableTest
    {
        private IQueryable<Item> _items;

        [SetUp]
        public void Setup()
        {
            _items = new Fixture().CreateMany<Item>(30).AsQueryable();
        }

        [Test]
        public void Should_return_if_there_is_previous_page()
        {
            var query = new Query { Page = 1, PageSize = 5 };
            query.Results = _items.ToPaginate(query).ToList();
            query.HasPreviousPage().ShouldBeFalse();

            query.Page = 2;
            query.Results = _items.ToPaginate(query).ToList();
            query.HasPreviousPage().ShouldBeTrue();
        }
        
        public class Query : IPageable<Item>
        {
            public int Page { get; set; }
            public int PageSize { get; set; }
            public int Pages { get; set; }
            public int CountShowing { get; set; }
            public int CountTotal { get; set; }
            
            public IReadOnlyList<Item> Results { get; set; } = new Collection<Item>();
        }

        public class Item
        {
        }
    }
}