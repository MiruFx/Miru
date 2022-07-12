using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Miru.Mvc;

public class MiruControllerFeatureProvider : ControllerFeatureProvider
{
    private readonly Func<TypeInfo, bool> _filter = typeInfo => true;

    public MiruControllerFeatureProvider()
    {
    }
        
    public MiruControllerFeatureProvider(Type outerType)
    {
        _filter = typeInfo =>
        {
            if (typeInfo.ReflectedType != null && typeInfo.ReflectedType == outerType)
                return true;
                
            if (typeInfo.ReflectedType?.ReflectedType != null && typeInfo.ReflectedType?.ReflectedType == outerType)
                return true;
                
            return false;
        };
    }

    protected override bool IsController(TypeInfo typeInfo)
    {
        // IsController could be one line only. Just split here for debug purposes 
        var isNested = IsNestedController(typeInfo);
                
        if (!isNested) 
            return false;
                    
        return _filter(typeInfo);
    }

    private static bool IsNestedController(TypeInfo typeInfo)
    {
        return typeInfo.IsClass &&
               !typeInfo.IsAbstract &&
               !typeInfo.ContainsGenericParameters &&
               !typeInfo.IsPublic && 
               typeInfo.IsNested &&
               typeInfo.Name.EndsWith(nameof(Controller));
    }
}