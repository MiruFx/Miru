using System;
using System.Collections.Generic;

namespace Miru.Mvc
{
    public class SelectLookups : List<Lookup>
    {
        public static SelectLookups FromEnum<TEnum>() where TEnum : struct, Enum =>
            Enum.GetValues<TEnum>().ToSelectLookups();
    }
}