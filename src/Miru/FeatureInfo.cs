using System;
using System.Collections.Generic;
using System.Linq;

namespace Miru;

public class FeatureInfo
{
    public static FeatureInfo For<TRequest>(TRequest request)
    {
        return new FeatureInfo(request.GetType());
    }
        
    public Type Type { get; }
    public object Instance { get; }

    public FeatureInfo(Type type)
    {
        // TODO: cache in a hashtable <type, featureinfo>
        Type = type;
    }
        
    public FeatureInfo(object instance) : this(instance.GetType())
    {
        Instance = instance;
    }

    public bool IsIn(string folder, string featureFolder = ".Features.")
    {
        return Type.Namespace.Contains($"{featureFolder}{folder}");
    }

    public bool Implements<T>()
    {
        return Type.Implements<T>() || Type.ReflectedType!.Implements<T>();
    }
    
    public string GetIdsQueryString()
    {
        var propertyValues = new Dictionary<string, object>();
        
        var properties = Instance.GetType().GetProperties();

        foreach (var property in properties)
        {
            if (property.Name.EndsWith("Id") && property.PropertyType == typeof(long))
            {
                propertyValues[property.Name] = property.GetValue(Instance);
            }
        }

        return propertyValues.Select(x => $"{x.Key}={x.Value}").Join("&");
    }
    
    public string GetIdsProperties()
    {
        var propertyValues = new Dictionary<string, object>();
        
        var properties = Instance.GetType().GetProperties();

        foreach (var property in properties)
        {
            if (property.Name.EndsWith("Id") && property.PropertyType == typeof(long))
            {
                propertyValues[property.Name] = property.GetValue(Instance);
            }
        }

        return propertyValues.Select(x => $"{x.Key}: {x.Value}").Join(", ");
    }
    
    public string GetTitle()
    {
        var name = GetName();
        
        return $"{name}?{GetIdsQueryString()}";
    }
    
    public string GetName()
    {
        var reflectedType = Type.ReflectedType;
    
        if (reflectedType is not null)
            return reflectedType.Name;
            
        return Type.Name;
    }
}