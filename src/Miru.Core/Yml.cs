using System;
using System.Collections.Generic;
using System.Linq;
using Baseline;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Miru.Core;

public static class Yml
{
    public static List<Func<IPropertyDescriptor, bool>> PropertyFilters = new();
    
    public static Lazy<ISerializer> Serializer = new(() =>
    {
        return new SerializerBuilder()
            .IgnoreFields()
            .ConfigureDefaultValuesHandling(
                DefaultValuesHandling.OmitNull |
                DefaultValuesHandling.OmitEmptyCollections |
                DefaultValuesHandling.OmitDefaults)
            .WithTypeInspector(inspector => new Yml.FilterPropertiesInspector(inspector))
            .Build();
    });
    
    public static Lazy<IDeserializer> Deserializer = new(() =>
    {
        return new DeserializerBuilder()
            .IgnoreFields()
            .WithTypeInspector(inspector => new Yml.FilterPropertiesInspector(inspector))
            .Build();
    });

    public static string Dump<T>(T value)
    {
        if (value == null)
            return "null";

        var yaml = ToYml(value);

        if (yaml.StartsWith("{}") || yaml.IsEmpty())
            return "Empty";

        return yaml;
    }

    public static string ToYml<T>(T value) => 
        Serializer.Value.Serialize(value);

    public static T FromYml<T>(string content) =>
        Deserializer.Value.Deserialize<T>(content);
        
    public class FilterPropertiesInspector(ITypeInspector innerTypeDescriptor) : TypeInspectorSkeleton
    {
        public override string GetEnumName(Type enumType, string name) => 
            innerTypeDescriptor.GetEnumName(enumType, name);

        public override string GetEnumValue(object enumValue) => innerTypeDescriptor.GetEnumValue(enumValue);

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
        {
            var properties = innerTypeDescriptor.GetProperties(type, container)
                .Where(p => !p.Name.ContainsNoCase("password"))
                .Where(p => !(p.Name.ContainsNoCase("body") && type.FullName.Equals("Miru.Mailing.Email")));

            foreach (var filter in PropertyFilters)
                properties = properties.Where(filter);

            return properties;
        }
    }
}