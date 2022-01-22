using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Miru.Core;

public class YmlConfig
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
            .WithTypeInspector(inspector => new YmlExtensions.FilterPropertiesInspector(inspector))
            .Build();
    });
    
    public static Lazy<IDeserializer> Deserializer = new(() =>
    {
        return new DeserializerBuilder()
            .IgnoreFields()
            .WithTypeInspector(inspector => new YmlExtensions.FilterPropertiesInspector(inspector))
            .Build();
    });
}

public static class YmlExtensions
{
    public static string ToYml<T>(this T value) => 
        YmlConfig.Serializer.Value.Serialize(value);

    public static T FromYml<T>(this string content) =>
        YmlConfig.Deserializer.Value.Deserialize<T>(content);
        
    public class FilterPropertiesInspector : TypeInspectorSkeleton
    {
        private readonly ITypeInspector _innerTypeDescriptor;

        public FilterPropertiesInspector(ITypeInspector innerTypeDescriptor)
        {
            _innerTypeDescriptor = innerTypeDescriptor;
        }

        public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
        {
            var properties = _innerTypeDescriptor.GetProperties(type, container)
                .Where(p => !p.Name.ContainsNoCase("password"))
                .Where(p => !(p.Name.ContainsNoCase("body") && type.FullName.Equals("Miru.Mailing.Email")));

            foreach (var filter in YmlConfig.PropertyFilters)
                properties = properties.Where(filter);

            return properties;
        }
    }
}