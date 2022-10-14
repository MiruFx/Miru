// using AV.Enumeration;
// using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
//
// namespace Miru.Databases.EntityFramework;
//
// public class EnumerationValueConverter<TEnumeration> : ValueConverter<TEnumeration, int>
//     where TEnumeration : Enumeration
// {
//     public EnumerationValueConverter()
//         : base(e => e.Value, value => Enumeration.FromValue<TEnumeration>(value))
//     {
//     }
// }