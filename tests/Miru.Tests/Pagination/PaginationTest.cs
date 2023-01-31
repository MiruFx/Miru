using System.Collections.Generic;
using Miru.Pagination;

namespace Miru.Tests.Pagination;

[TestFixture]
public class PaginationTest
{
    [Test]
    public void Return_if_there_is_pagination()
    {
        new Result { Pages = 1 }.HasPagination().ShouldBeFalse();
            
        new Result { Pages = 2 }.HasPagination().ShouldBeTrue();
    }
        
    [Test]
    public void Return_if_there_is_previous_page()
    {
        new Result { Results = new List<string>() }.HasPreviousPage().ShouldBeFalse();
            
        new Result { Results = SomeStrings(), Page = 1 }.HasPreviousPage().ShouldBeFalse();
            
        new Result { Results = SomeStrings(), Page = 2 }.HasPreviousPage().ShouldBeTrue();
    }
        
    [Test]
    public void Return_if_there_is_next_page()
    {
        new Result { Results = new List<string>() }.HasNextPage().ShouldBeFalse();
            
        new Result { Results = SomeStrings(), Page = 1, Pages = 1 }.HasNextPage().ShouldBeFalse();
            
        new Result { Results = SomeStrings(), Page = 2, Pages = 10 }.HasNextPage().ShouldBeTrue();
    }
        
    private List<string> SomeStrings()
    {
        return new List<string> { "some", "set", "of", "strings" };
    }

    public class Result : IPageable<string>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Pages { get; set; }
        public int CountShowing { get; set; }
        public int CountTotal { get; set; }
        public IReadOnlyList<string> Results { get; set; } = new List<string>();
    }
}