using System;
using System.Collections.Generic;
using HtmlTags.Reflection;

namespace Miru.Scoping
{
    public interface IScopeFor<TRequest>
    {
        IEnumerable<Type> Scopes { get; }
        
        IEnumerable<Accessor> Accessors { get; }
    }
}