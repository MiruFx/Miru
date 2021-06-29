using System;

namespace Miru
{
    public class FeatureInfo
    {
        public static FeatureInfo For<TRequest>(TRequest request)
        {
            return new FeatureInfo(request.GetType());
        }
        
        private readonly Type _type;

        public FeatureInfo(Type type)
        {
            _type = type;
        }
        
        public FeatureInfo(object instance)
        {
            _type = instance.GetType();
        }

        public bool IsIn(string featureFolder)
        {
            return _type.Namespace.Contains($".Features.{featureFolder}");
        }

        public bool Implements<T>()
        {
            return _type.Implements<T>() || _type.ReflectedType!.Implements<T>();
        }
    }
}