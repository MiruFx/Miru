using System;
using System.Collections.Generic;

namespace Miru.Diagnostics
{
    public class MiruMiniProfilerOptions
    {
        private readonly List<Type> _ignores = new();

        public void Ignore<T>()
        {
            var type = typeof(T);
            
            if (_ignores.Contains(type) == false)
                _ignores.Add(type);
        }

        public bool ShouldIgnore<T>(T response) => _ignores.Contains(response.GetType());
    }
}