using System.Collections.Generic;

namespace Miru.Mvc;

public class SelectLookups : List<Lookup>
{
    public static SelectLookups For<T>(params T[] options) =>
        options.ToSelectLookups(x => x, x => x);

    public static SelectLookups Empty = new();
    
    public static SelectLookups ForEnum<TEnum>() where TEnum : struct, Enum =>
        Enum.GetValues<TEnum>().ToSelectLookups();
    
    public SelectLookups AddFirst(object id, string text)
    {
        Insert(0, new Lookup(id, text));
        return this;
    }
}

public static class SelectLookupsExtensions
{
    public static SelectLookups ToSelectLookups(this Range range)
    {
        var selectLookups = new SelectLookups();

        for (var i = range.Start.Value; i <= range.End.Value; i++)
            selectLookups.Add(new Lookup(i, i));

        return selectLookups;
    }
    
    public static SelectLookups ToSelectLookups(this string[] items)
    {
        var selectLookups = new SelectLookups();

        foreach(var item in items)
            selectLookups.Add(new Lookup(item, item));

        return selectLookups;
    }
}