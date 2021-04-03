using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Miru.Domain;

namespace Miru.Databases.EntityFramework
{
    public class EnumerationValueConverter<TEnumeration, TId> : ValueConverter<TEnumeration, TId>
        where TEnumeration : Enumeration<TEnumeration, TId> where TId : IComparable
    {
        public EnumerationValueConverter()
            : base(e => e.Value, value => Enumeration<TEnumeration, TId>.FromValue(value))
        {
        }
    }
}