using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;

namespace Miru.Core
{
    public static class YmlExtensions
    {
        public static string ToYml<T>(this T value)
        {
            // TODO: cache builder
            return new SerializerBuilder()
                .IgnoreFields()
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
                .WithTypeInspector(inspector => new FilterPropertiesInspector(inspector))
                .Build()
                .Serialize(value);
        }

        public static T FromYml<T>(this string content)
        {
            // TODO: cache builder
            return new DeserializerBuilder()
                .IgnoreFields()
                .WithTypeInspector(inspector => new FilterPropertiesInspector(inspector))
                .Build()
                .Deserialize<T>(content);
        }
        
        // FIXME: Make it global configuration
        public class FilterPropertiesInspector : TypeInspectorSkeleton
        {
            private readonly ITypeInspector _innerTypeDescriptor;

            public FilterPropertiesInspector(ITypeInspector innerTypeDescriptor)
            {
                _innerTypeDescriptor = innerTypeDescriptor;
            }

            public override IEnumerable<IPropertyDescriptor> GetProperties(Type type, object container)
            {
                return _innerTypeDescriptor.GetProperties(type, container)
                    .Where(p => 
                        !p.Name.ContainsNoCase("password") && 
                        !(p.Name.ContainsNoCase("body") && type.FullName.Equals("Miru.Mailing.Email")));
            }
        }
    }
}