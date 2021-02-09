using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HtmlTags.Reflection;

namespace Miru.Scoping
{
    public class ScopeFor<TRequest> : IScopeFor<TRequest>
    {
        private readonly List<Type> _scopes = new();
        
        private readonly List<Accessor> _accessors = new();

        public IEnumerable<Type> Scopes => _scopes.ToList();
        
        public IEnumerable<Accessor> Accessors => _accessors.ToList();
        
        protected void AddScope<TScope>()
        {
            _scopes.Add(typeof(TScope));
        }

        protected void AddIntoView<TProperty>(Expression<Func<TRequest, TProperty>> addPropertyIntoViewBag)
        {
            _accessors.Add(addPropertyIntoViewBag.ToAccessor());
        }
    }
}