using System.Collections.Immutable;
using System.IO;
using Baseline;
using Baseline.Dates;
using Bogus;
using Miru.Domain;

namespace Miru.Testing;

[ShouldlyMethods]
public static class ShouldlyExtensions
{
    /// <summary>
    /// It compares two DateTime having 1 second difference of tolerance
    /// </summary>
    public static void ShouldBeAprox(this DateTime current, DateTime expected) =>
        current.ShouldBe(expected, TimeSpan.FromSeconds(1));
    
    /// <summary>
    /// It compares only two dates from DateTimes 
    /// </summary>
    public static void DateShouldBe(this DateTime current, DateTime expected) =>
        current.Date.ShouldBe(expected.Date);
    
    /// <summary>
    /// Compares two decimal values ignoring 0.01 difference
    /// </summary>
    public static void ShouldBeAprox(this decimal current, decimal value) => 
        current.ShouldBe(value, 0.01m);

    public static void ShouldBeSecondsAgo(this DateTime date, int seconds = 3)
    {
        date.ShouldBeGreaterThanOrEqualTo(seconds.Seconds().Ago());
    }
        
    public static void ShouldBeSecondsAgo(this DateTime? date, int seconds = 3)
    {
        date.GetValueOrDefault().ShouldBeGreaterThanOrEqualTo(seconds.Seconds().Ago());
    }
        
    public static void ShouldBeAt(this DateTime actual, DateTime expected)
    {
        actual.ShouldBe(expected, 1.Seconds());
    }
        
    public static void ShouldBeAt(this DateTime? actual, DateTime expected)
    {
        actual.GetValueOrDefault().ShouldBe(expected, 1.Seconds());
    }
        
    public static void ShouldCount<T>(this IEnumerable<T> actual, int expected)
    {
        var actualCount = actual.Count();
        var expectedCount = expected;
            
        actualCount.ShouldBe(expected, 
            $"Number of items of {typeof(T).FullName} should be {actualCount} but was {expectedCount}");
    }

    public static void ShouldMatchOrderedIds(this IEnumerable<IHasId> left, params IHasId[] right)
    {
        var leftIds = left.Select(x => x.Id).ToImmutableList();
        var rightIds = right.Select(x => x.Id).ToImmutableList();

        if (leftIds.Count != rightIds.Count)
            throw new AssertionException($@"Left count ({leftIds.Count}) doesn't match with Right count ({rightIds.Count}):

    Left Ids: {leftIds.Join(",")}
    Right Ids: {rightIds.Join(",")}
");
            
        for (int i = 0; i < leftIds.Count; i++)
        {
            if (leftIds[i] != rightIds[i])
                throw new AssertionException($@"Left doesn't match with Right at position {i}:

    Left Ids: {leftIds.Join(",")}
    Right Ids: {rightIds.Join(",")}
");
        }
    }

    public static void ShouldMatchIds(this IEnumerable<IHasId> left, IEnumerable<IHasId> right)
    {
        left.ShouldMatchIds(right.ToArray());
    }
        
    public static void ShouldMatchIds(this IEnumerable<IHasId> left, params IHasId[] right)
    {
        var leftIds = left.Select(x => x.Id).ToImmutableList();
        var rightIds = right.Select(x => x.Id).ToImmutableList();

        if (ScrambledEquals(leftIds, rightIds) == false)
        {
            throw new AssertionException($@"Left doesn't match with Right:

    Left Ids: {leftIds.Join(",")}
    Right Ids: {rightIds.Join(",")}
");
        }
    }
        
    public static void ShouldMatchOrderedIds(this IEnumerable<IHasId> left, IEnumerable<IHasId> right)
    {
        left.ShouldMatchOrderedIds(right.ToArray());
    }

    public static void ItemsShouldBe<T>(this IEnumerable<T> actual, params T[] expected) 
    {
        var actualList = actual.ToList();
            
        var actualCount = actualList.Count;
        var expectedCount = expected.Length;
            
        actualCount.ShouldBe(expected.Length, 
            $"Number of items of {typeof(T).FullName} should be {actualCount} but was {expectedCount}");

        for (int i = 0; i < actualCount; i++)
        {
            actualList[i].ShouldBe(expected[i], 
                $"Expected item at position {i} of {typeof(T).FullName} is different from actual");
        }
    }
        
    public static void ShouldContainAll<T>(this IEnumerable<T> actual, params T[] expected)
    {
        var actualList = actual.ToList();
            
        var actualCount = actualList.Count;
        var expectedCount = expected.Length;
            
        actualCount.ShouldBe(expected.Length, 
            $"Number of items of {typeof(T).FullName} should be {actualCount} but was {expectedCount}");

        for (int i = 0; i < actualCount; i++)
        {
            expected.Any(m => m.Equals(actualList[i])).ShouldBeTrue($"Should contain {actualList[i]}");
        }
    }
        
    public static void ShouldExist(this MiruPath fileName)
    {
        File.Exists(fileName).ShouldBeTrue($"File {fileName} should exist but it doesn't");
    }
    
    public static void ShouldNotExist(this MiruPath fileName)
    {
        File.Exists(fileName).ShouldBeFalse($"File {fileName} should NOT exist but it does");
    }

    public static void ShouldBeEmpty(this MiruPath fileName)
    {
        fileName.ShouldExist();
        fileName.FileSize().ShouldBe(0);    
    }

    public static void ShouldBeSameAs<T>(this T source, T other) where T : class, IComparableWith<T>
    {
        if (source.IsSameAs(other) == false)
            throw new ShouldAssertException("The two IComparableWith objects are not the same");
    }

    private static bool ScrambledEquals<T>(IEnumerable<T> left, IEnumerable<T> right)
    {
        return !left.Except(right).Union(right.Except(left)).Any();
    }
    
    public static void DateShouldBe(this DateTime? current, DateTime expected) =>
        current!.Value.Date.ShouldBe(expected.Date);
    
    public static void DateShouldBe(this DateTime? current, DateTime? expected) =>
        current!.Value.Date.ShouldBe(expected.GetValueOrDefault().Date);
}