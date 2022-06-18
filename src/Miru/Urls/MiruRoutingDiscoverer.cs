using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Miru.Core;

namespace Miru.Urls;

public class MiruRoutingDiscoverer : IApplicationModelConvention
{
    private readonly Dictionary<string, ModelToUrlMap> _mappings;

    public Dictionary<string, ModelToUrlMap> Mappings => _mappings;
        
    public MiruRoutingDiscoverer(Dictionary<string, ModelToUrlMap> mappings)
    {
        _mappings = mappings;
    }

    private AttributeRouteModel GetRouteModel(IList<SelectorModel> selectors) => selectors
        .Where(x => x.AttributeRouteModel != null)
        .Select(x => x.AttributeRouteModel)
        .FirstOrDefault();
        
    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers)
        {
            // is considered Miru's Controller only if it's inside a feature
            if (controller.ControllerType.IsNested == false)
                continue;

            var controllerRoute = GetRouteModel(controller.Selectors);
                
            if (controllerRoute == null)
            {
                var routeAttribute = new RouteAttribute(controller.ControllerName);

                controller.Selectors[0].AttributeRouteModel = new AttributeRouteModel(routeAttribute);
            }
                
            foreach (var controllerAction in controller.Actions)
            {
                var route = GetRouteModel(controllerAction.Selectors);

                if (route == null)
                {
                    var routeAttribute = new RouteAttribute(controllerAction.ActionName.IfThen("Index", string.Empty));
                    
                    route = new AttributeRouteModel(routeAttribute);

                    controllerAction.Selectors[0].AttributeRouteModel = route;
                }
                    
                var oneParam = controllerAction.Parameters.FirstOrDefault();
                    
                if (oneParam != null && 
                    oneParam.ParameterInfo.ParameterType.GetTypeInfo().IsClass && 
                    oneParam.ParameterInfo.ParameterType != typeof(string))
                {
                    route.Name = oneParam.ParameterInfo.ParameterType.ToString();

                    var map = new ModelToUrlMap
                    {
                        Method = controllerAction.Attributes.OfType<HttpPostAttribute>().Any() ? HttpMethod.Post : HttpMethod.Get,
                        ActionName = controllerAction.ActionName,
                        ControllerName = controller.ControllerName
                    };
                        
                    _mappings[oneParam.ParameterInfo.ParameterType.ToString()] = map;

                    // if (oneParam.ParameterInfo.ParameterType.DeclaringType != null)
                    //     _mappings[oneParam.ParameterInfo.ParameterType.DeclaringType] = map;
                }
            }
        }
    }
}