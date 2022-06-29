using System.Collections.Generic;
using Miru.Domain;

namespace Miru.Tests.Testing;

public class MatchExtensionsTest
{
    [Test]
    public void Should_assert_if_ids_match_in_any_order()
    {
        // arrange
        var products = new List<Product>()
        {
            new() { Id = 1 },
            new() { Id = 2 },
            new() { Id = 3 }
        };
     
        // act & assert
        new List<ProductView>()
        {
            new() { Id = 3 },
            new() { Id = 2 },
            new() { Id = 1 },
        }.ShouldMatchIds(products);

        Should.Throw<AssertionException>(() =>
        {
            new List<ProductView>()
            {
                new() { Id = 0 },
                new() { Id = 2 },
                new() { Id = 1 },
            }.ShouldMatchIds(products);
        });
    }

    public class Product : Entity
    {
    }

    public class ProductView : IHasId
    {
        public long Id { get; set; }
    }
}