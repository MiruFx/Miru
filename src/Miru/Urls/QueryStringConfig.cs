using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Miru.Core;
using Miru.Domain;
using Miru.Pagination;

namespace Miru.Urls
{
    public class QueryStringConfig
    {        
        private readonly List<Func<PropertyInfo, object, bool>> _queryStringBuilderFilter = new List<Func<PropertyInfo, object, bool>>();
        private readonly Dictionary<Type, Func<PropertyInfo, object, bool>> _accessorFilters = new Dictionary<Type,Func<PropertyInfo, object, bool>>();
        private Func<PropertyInfo, bool> _ignoredModifiedFilters;

        public QueryStringConfig()
        {
            // TODO: better api
            
            FilterByAccessorFor<IPageable>((prop, value) => 
                !prop.Name.CaseCmp(nameof(IPageable.CountShowing)) &&
                !prop.Name.CaseCmp(nameof(IPageable.CountTotal)) &&
                !prop.Name.CaseCmp(nameof(IPageable.Pages)) && 
                !(prop.Name.CaseCmp(nameof(IPageable.PageSize)) && value.ToInt() == PaginationConfig.DefaultPageSize) &&
                !(prop.Name.CaseCmp(nameof(IPageable.Page)) && value.ToInt() == 1));
                
            QueryStringBuilderFilter((prop, value) => 
                !prop.Name.StartsWith("Result.") && 
                !prop.Name.StartsWith("Results.") && 
                !prop.Name.CaseCmp("Result") && 
                !prop.Name.CaseCmp("Results") && 
                !prop.Name.StartsWith("_") &&
                !prop.PropertyType.Implements<IEntity>());

            IgnoreModifiedWhen(prop =>
                prop.DeclaringType.Implements<IPageable>() && prop.Name.Equals(nameof(IPageable.Page)) ||
                prop.DeclaringType.Implements<IPageable>() && prop.Name.Equals(nameof(IPageable.PageSize)));
            
            // QueryStringBuilderFilter((prop, cmd) => !prop.DeclaringType.IsRequestCommand());
        }

        public IEnumerable<Func<PropertyInfo, object, bool>> GetQueryStringBuilderFilters<T>(T input)
        {
            foreach (var filter in _accessorFilters.Where(_ => _.Key.IsInstanceOfType(input)))
                yield return filter.Value;

            foreach (var filter in _queryStringBuilderFilter)
                yield return filter;
        }
        
        public Func<PropertyInfo, bool> GetIgnoredWhenModified()
        {
            return _ignoredModifiedFilters;
        }

        public QueryStringConfig QueryStringBuilderFilter(Func<PropertyInfo, object, bool> filter)
        {
            _queryStringBuilderFilter.Add(filter);
            return this;
        }

        public QueryStringConfig FilterByAccessorFor<TModel>(Func<PropertyInfo, object, bool> filter)
        {
            _accessorFilters.AddOrUpdate(typeof(TModel), filter);
            return this;
        }

        public QueryStringConfig IgnoreModifiedWhen(Func<PropertyInfo, bool> filter)
        {
            _ignoredModifiedFilters = filter;
            return this;
        }
    }
}