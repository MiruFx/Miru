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

        public bool IsIn(string folder)
        {
            return _type.Namespace.Contains($".Features.{folder}");
        }

        public bool Implements<T>()
        {
            return _type.Implements<T>() || _type.ReflectedType!.Implements<T>();
        }
    }
}