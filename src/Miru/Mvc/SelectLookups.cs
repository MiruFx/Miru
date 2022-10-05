using System;
using System.Collections.Generic;

namespace Miru.Mvc;

public class SelectLookups : List<Lookup>
{
    public static SelectLookups FromEnum<TEnum>() where TEnum : struct, Enum =>
        Enum.GetValues<TEnum>().ToSelectLookups();

    public SelectLookups AddFirst(object id, string text)
    {
        Insert(0, new Lookup(id, text));
        return this;
    }
}