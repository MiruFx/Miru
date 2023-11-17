using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Miru.Mailing;

namespace Miru;

public class FeatureInfo
{
    private const string FeatureNamespace = ".Features.";
    
    public Type Type { get; }
    public object Instance { get; }

    public static FeatureInfo For<TRequest>(TRequest request)
    {
        return new FeatureInfo(request.GetType());
    }

    public FeatureInfo(Type type)
    {
        // TODO: cache in a hashtable <type, featureinfo>
        Type = type;
    }
        
    public FeatureInfo(object instance) : this(instance.GetType())
    {
        // TODO: cache in a hashtable <type, featureinfo>
        Instance = instance;
    }

    public bool IsIn(string folder, string featureFolder = ".Features.")
    {
        return Type.Namespace.Contains($"{featureFolder}{folder}");
    }

    public string FeatureGroup => GetFeatureGroup(Type.Namespace);
    
    public static string GetFeatureGroup(string nameSpace)
    {
        var featuresIndexOf = nameSpace.IndexOf(FeatureNamespace, StringComparison.InvariantCulture);
        var featureOfLength = FeatureNamespace.Length;
        
        return nameSpace.Substring(featuresIndexOf + featureOfLength);
    }

    public bool Implements<T>()
    {
        return Type.Implements<T>() || Type.ReflectedType!.Implements<T>();
    }
    
    public string GetIdsQueryString()
    {
        var propertyValues = new Dictionary<string, object>();
        
        var properties = Instance.GetType().GetProperties();

        if (Instance is EmailJob emailJob)
        {
            propertyValues.Add("Subject", emailJob.Email.Subject);
            propertyValues.Add("To", emailJob.Email.ToAddresses.Join(","));
        }
        else
        {
            foreach (var property in properties)
            {
                if (property.Name.EndsWith("Id") && property.PropertyType == typeof(long))
                {
                    propertyValues[property.Name] = property.GetValue(Instance);
                }
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

        var queryString = GetIdsQueryString();

        if (string.IsNullOrEmpty(queryString))
            return name;
        
        return $"{name}?{queryString}";
    }
    
    public string GetName()
    {
        var reflectedType = Type.ReflectedType;
    
        if (reflectedType is not null)
            return reflectedType.Name;
            
        return Type.Name;
    }
    
    /// <summary>
    /// It looks into inspected class and also the parent for the TAttribute
    /// </summary>
    public static TAttribute GetFeatureAttribute<TAttribute>(object request) where TAttribute : Attribute
    {
        var attribute = request.GetType().GetAttribute<TAttribute>();

        if (attribute != null)
        {
            return attribute;
        }

        return request.GetType().ReflectedType?.GetCustomAttribute<TAttribute>();
    }
}