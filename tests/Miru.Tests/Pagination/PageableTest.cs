using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoFixture;
using Miru.Pagination;

namespace Miru.Tests.Pagination;

public class PageableTest
{
    private IQueryable<Item> _items;

    [SetUp]
    public void Setup()
    {
        _items = new Fixture().CreateMany<Item>(20).AsQueryable();
    }

    [Test]
    public void Should_return_if_there_is_previous_page()
    {
        var query = new Query { Page = 1, PageSize = 2 };
        query.Results = _items.ToPaginate(query).ToList();
        query.HasPreviousPage().ShouldBeFalse();

        query.Page = 10;
        query.Results = _items.ToPaginate(query).ToList();
        query.HasPreviousPage().ShouldBeTrue();
    }
        
    [Test]
    public void Should_return_if_there_is_next_page()
    {
        var query = new Query { Page = 10, PageSize = 2 };
        query.Results = _items.ToPaginate(query).ToList();
        query.HasNextPage().ShouldBeFalse();

        query.Page = 1;
        query.Results = _items.ToPaginate(query).ToList();
        query.HasNextPage().ShouldBeTrue();
    }

    [Test]
    public void Should_return_pager()
    {
        // arrange
        var list = new Fixture().CreateMany<Item>(20).AsQueryable();
        var query = new Query
        {
            Page = 5, 
            PageSize = 2
        };
            
        query.Results = list.ToPaginate(query).ToList();
            
        // act & assert
        query.Pager(7).Pages.ItemsShouldBe(2, 3, 4, 5, 6, 7, 8);
        query.Pager(5).Pages.ItemsShouldBe(3, 4, 5, 6, 7);
        query.Pager(3).Pages.ItemsShouldBe(4, 5, 6);
        query.Pager(1).Pages.ItemsShouldBe(5);
        query.Pager(20).Pages.ItemsShouldBe(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
    }
    
    [Test]
    public void Should_return_skip_index()
    {
        // arrange
        var query1 = new Query { Page = 1, PageSize = 10 };
        var query2 = new Query { Page = 2, PageSize = 10 };
        
        // act
        query1.Results = _items.ToPaginate(query1).ToList();
        query2.Results = _items.ToPaginate(query2).ToList();
        
        // assert
        query1.Skip().ShouldBe(0);
        query2.Skip().ShouldBe(10);
    }
    
    [Test]
    public void Should_paginate_without_count()
    {
        // arrange
        var query1 = new Query { Page = 1, PageSize = 10 };
        var query2 = new Query { Page = 2, PageSize = 10 };
        
        // act
        query1.Paginate();
        query2.Paginate();
        
        // assert
        query1.Skip().ShouldBe(0);
        query2.Skip().ShouldBe(10);
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