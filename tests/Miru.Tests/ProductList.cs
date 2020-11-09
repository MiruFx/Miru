using System.Collections.Generic;

namespace Miru.Tests
{
    public class ProductList
    {
        public class Query
        {
            public string Title => "This is the title";
            public long CategoryId { get; set; }
            public string Category { get; set; }
            public bool OnSales { get; set; }
            public Result Result { get; set; } = new Result();
            public List<Size> Size { get; set; } = new List<Size>();
        }

        public enum Size
        {
            None = 0,
            Small = 1,
            Medium = 2,
            Large = 3
        }
        
        public class Result
        {
            public int Total { get; set; }
        }
    }
}