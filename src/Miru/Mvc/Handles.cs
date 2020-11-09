using System;

namespace Miru.Mvc
{
    public class Handles : Attribute
    {
        public Type Type { get; set; }

        public Handles(Type type) => Type = type;
    }
}