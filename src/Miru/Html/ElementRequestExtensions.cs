using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;
using HtmlTags;
using HtmlTags.Conventions;
using Miru.Validation;
using Miru.Core;

namespace Miru.Html;

public static class ElementRequestExtensions
{
    public static bool Contains(this string value, params string[] possibilities) =>
        possibilities.Any(value.Contains);
        
    public static bool PropertyContainNames(
        this ElementRequest elementRequest,
        params string[] names) =>
            elementRequest.Accessor.Name.Contains(names);
    
    public static object Get(this ElementRequest elementRequest, Type type)
    {
        var sp = elementRequest.Get<IServiceProvider>();
        var service = sp.GetService(type);

        return service;
    }
        
    public static object FindModel(this ElementRequest elementRequest)
    {
        if (elementRequest.Accessor is ModelMetadataAccessor modelMetadataAccessor)
        {
            return modelMetadataAccessor.ModelExpression.ModelExplorer.Container.Model;
        }

        return elementRequest.Model;
    }
        
    public static string FindPropertyFullName(this ElementRequest elementRequest)
    {
        if (elementRequest.Accessor is ModelMetadataAccessor modelMetadataAccessor)
        {
            return modelMetadataAccessor.ModelExpression.Name;
        }
            
        return elementRequest.Accessor.Name;
    }

    public static ValidatorRules ValidatorRules(this ElementRequest elementRequest)
    {
        var command = elementRequest.FindModel();
            
        var validator = (IValidator) elementRequest.Get(typeof(IValidator<>).MakeGenericType(command.GetType()));

        if (validator == null)
            return new ValidatorRules(new List<IPropertyValidator>());

        string propertyFullName = elementRequest.FindPropertyFullName();

        var builder = validator.RulesFor(propertyFullName);

        if (builder.Has<IPropertyValidator>())
        {
            return builder;
        }

        // the property's owner type has a specialized validator
        validator = (IValidator) elementRequest.Get(typeof(IValidator<>).MakeGenericType(elementRequest.HolderType()));
            
        if (validator == null)
            return new ValidatorRules(new List<IPropertyValidator>());
            
        var propertyName = elementRequest.Accessor.Name;
            
        return validator.RulesFor(propertyName);
    }
}