using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Miru;

public interface IMiruApp : IDisposable
{
    /// <summary>
    /// Get instance of T. If can't find T, an Exception is thrown
    /// </summary>
    T Get<T>();
        
    /// <summary>
    /// Try get an instance of T. If can't find T, return null
    /// </summary>
    T TryGet<T>();
        
    /// <summary>
    /// Get instance of a type. If can't find instance, an Exception is thrown
    /// </summary>
    object Get(Type type);
        
    IEnumerable<T> GetAll<T>();
        
    Task RunAsync();
}