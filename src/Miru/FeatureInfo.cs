using System;

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
}

public static class FeatureInfoExtensions
{
    public static FeatureInfo ToFeatureInfo<T>(this T model) => new FeatureInfo(model);
}