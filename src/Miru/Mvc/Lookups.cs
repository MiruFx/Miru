using System;
using System.Collections.Generic;

namespace Miru.Mvc
{
    public class Lookups : List<Lookup>
    {
        public static Lookups FromEnum<TEnum>() where TEnum : struct, Enum =>
            Enum.GetValues<TEnum>().ToLookups();
    }
}