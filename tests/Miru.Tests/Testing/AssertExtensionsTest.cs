using Miru.Domain;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Testing
{
    public class AssertExtensionsTest
    {
        [Test]
        public void Should_match_ids_of_two_lists_in_same_order()
        {
            // arrange
            var list1 = new[] { new Foo(1), new Foo(2), new Foo(3) };
            var match = new[] { new Foo(1), new Foo(2), new Foo(3) };
            var doesntMatchOrder = new[] { new Foo(3), new Foo(2), new Foo(1) };
            var doesntMatch1 = new[] { new Foo(1), new Foo(2), new Foo(4) };
            var doesntMatch2 = new[] { new Foo(1), new Foo(2), new Foo(3), new Foo(4) };
            
            // act & assert
            list1.ShouldMatchOrderedIds(match);
            Should.Throw<AssertionException>(() => list1.ShouldMatchOrderedIds(doesntMatchOrder));
            Should.Throw<AssertionException>(() => list1.ShouldMatchOrderedIds(doesntMatch1));
            Should.Throw<AssertionException>(() => list1.ShouldMatchOrderedIds(doesntMatch2));
        }
        
        [Test]
        public void Should_match_ids_of_two_lists()
        {
            // arrange
            var list1 = new[] { new Foo(1), new Foo(2), new Foo(3) };
            var match = new[] { new Foo(1), new Foo(2), new Foo(3) };
            var matchOtherOrder = new[] { new Foo(3), new Foo(2), new Foo(1) };
            var doesntMatch1 = new[] { new Foo(1), new Foo(2), new Foo(4) };
            var doesntMatch2 = new[] { new Foo(1), new Foo(2), new Foo(3), new Foo(4) };
            
            // act & assert
            list1.ShouldMatchIds(match);
            list1.ShouldMatchIds(matchOtherOrder);
            Should.Throw<AssertionException>(() => list1.ShouldMatchOrderedIds(doesntMatch1));
            Should.Throw<AssertionException>(() => list1.ShouldMatchOrderedIds(doesntMatch2));
        }

        public class Foo : Entity
        {
            public Foo(int id) => Id = id;
        }
    }
}