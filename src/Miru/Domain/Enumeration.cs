/*
https://github.com/HeadspringLabs/Enumeration
*/
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Miru.Domain
{
    // [Serializable]
    // [DebuggerDisplay("{Name} - {Value}")]
    // public abstract class Enumeration<TEnumeration> : Enumeration<TEnumeration, int>
    //     where TEnumeration : Enumeration<TEnumeration>
    // {
    //     protected Enumeration(int value, string name)
    //         : base(value, name)
    //     {
    //     }
    //
    //     public static TEnumeration FromInt32(int value)
    //     {
    //         return FromValue(value);
    //     }
    //
    //     public static bool TryFromInt32(int listItemValue, out TEnumeration result)
    //     {
    //         return TryParse(listItemValue, out result);
    //     }
    // }
    //
    // [Serializable]
    // [DebuggerDisplay("{Name} - {Value}")]
    // public abstract class Enumeration<TEnumeration, TValue> : 
    //     IComparable<TEnumeration>, 
    //     IEquatable<TEnumeration>
    //         where TEnumeration : Enumeration<TEnumeration, TValue>
    //         where TValue : IComparable
    // {
    //     private static readonly Lazy<TEnumeration[]> Enumerations = new(GetEnumerations);
    //
    //     [DataMember(Order = 1)]
    //     readonly string _name;
    //
    //     [DataMember(Order = 0)]
    //     readonly TValue _value;
    //
    //     protected Enumeration(TValue value, string name)
    //     {
    //         if (value == null)
    //         {
    //             throw new ArgumentNullException(nameof(value));
    //         }
    //
    //         _value = value;
    //         _name = name;
    //     }
    //
    //     public TValue Value => _value;
    //
    //     public string Name => _name;
    //
    //     public int CompareTo(TEnumeration other)
    //     {
    //         return Value.CompareTo(other == default(TEnumeration) ? default(TValue) : other.Value);
    //     }
    //
    //     public sealed override string ToString()
    //     {
    //         return Name;
    //     }
    //
    //     public static TEnumeration[] GetAll()
    //     {
    //         return Enumerations.Value;
    //     }
    //
    //     private static TEnumeration[] GetEnumerations()
    //     {
    //         Type enumerationType = typeof(TEnumeration);
    //         return enumerationType
    //             .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
    //             .Where(info => enumerationType.IsAssignableFrom(info.FieldType))
    //             .Select(info => info.GetValue(null))
    //             .Cast<TEnumeration>()
    //             .ToArray();
    //     }
    //
    //     public override bool Equals(object obj)
    //     {
    //         return Equals(obj as TEnumeration);
    //     }
    //
    //     public bool Equals(TEnumeration other)
    //     {
    //         return other != null && ValueEquals(other.Value);
    //     }
    //
    //     public override int GetHashCode()
    //     {
    //         return Value.GetHashCode();
    //     }
    //
    //     public static bool operator ==(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
    //     {
    //         return Equals(left, right);
    //     }
    //
    //     public static bool operator !=(Enumeration<TEnumeration, TValue> left, Enumeration<TEnumeration, TValue> right)
    //     {
    //         return !Equals(left, right);
    //     }
    //
    //     public static TEnumeration FromValue(TValue value)
    //     {
    //         return Parse(value, "value", item => item.Value.Equals(value));
    //     }
    //
    //     public static TEnumeration Parse(string name)
    //     {
    //         return Parse(name, "name", item => item.Name == name);
    //     }
    //
    //     static bool TryParse(Func<TEnumeration, bool> predicate, out TEnumeration result)
    //     {
    //         result = GetAll().FirstOrDefault(predicate);
    //         return result != null;
    //     }
    //
    //     private static TEnumeration Parse(object value, string description, Func<TEnumeration, bool> predicate)
    //     {
    //         TEnumeration result;
    //
    //         if (!TryParse(predicate, out result))
    //         {
    //             string message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(TEnumeration));
    //             throw new ArgumentException(message, "value");
    //         }
    //
    //         return result;
    //     }
    //
    //     public static bool TryParse(TValue value, out TEnumeration result)
    //     {
    //         return TryParse(e => e.ValueEquals(value), out result);
    //     }
    //
    //     public static bool TryParse(string name, out TEnumeration result)
    //     {
    //         return TryParse(e => e.Name == name, out result);
    //     }
    //
    //     protected virtual bool ValueEquals(TValue value)
    //     {
    //         return Value.Equals(value);
    //     }
    // }
}
