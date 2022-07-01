using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Miru.Html;
using Miru.Html.Tags;
using Miru.Testing;
using Miru.Urls;
using NUnit.Framework;

namespace Miru.Tests.Html.TagHelpers;

public class TagHelperTest
{
    protected IServiceProvider ServiceProvider { get; set; }
    
    protected virtual void HtmlConfiguration(HtmlConfiguration htmlConfig)
    {
    }
    
    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        var services = new ServiceCollection()
            .AddMiruHtml(HtmlConfiguration)
            .AddOptions()
            .ReplaceTransient<IAntiforgeryAccessor, TestingAntiForgeryAccessor>()
            .AddTransient<IUrlMaps, StubUrlMaps>()
            .AddTransient<UrlLookup>()
            .AddSingleton(new UrlOptions())
            .AddLogging()
            .AddMvcCore()
            .AddViews()
            .Services
            .Configure<UrlOptions>(x =>
            {
                x.Base = "https://mirufx.github.io";
            });
                
        ServiceProvider = services.BuildServiceProvider();
    }

    protected TTag CreateTagWithFor<TTag, TModel>(
        TTag tag,
        TModel model) where TTag : MiruForTagHelper, new()
    {
        tag.RequestServices = ServiceProvider;

        tag.For = MakeExpression(model);

        return tag;
    }

    protected TTag CreateTagWithFor<TTag, TModel, TProperty>(
        TTag tag,
        TModel model, 
        Expression<Func<TModel, TProperty>> expression = null) where TTag : MiruForTagHelper, new()
    {
        tag.RequestServices = ServiceProvider;

        tag.For = MakeExpression(model, expression);

        return tag;
    }
        
    protected TTag CreateTag<TTag>(TTag tag) 
        where TTag : MiruTagHelper, 
        new()
    {
        tag.RequestServices = ServiceProvider;

        return tag;
    }
    
    protected TTag CreateTagWithModel<TTag, TModel>(TTag tag, TModel model) 
        where TTag : MiruForTagHelper, 
        new()
    {
        tag.RequestServices = ServiceProvider;
        tag.Model = model;
        return tag;
    }

    protected TagHelperOutput ProcessTag2<TTag>(
        TTag tag, 
        string html,
        string content = "") where TTag : TagHelper
    {
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));

        var attributes = new TagHelperAttributeList();
            
        var output = new TagHelperOutput(
            html,
            attributes,
            (result, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetHtmlContent(content);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        tag.Process(context, output);

        return output;
    }
        
    protected async Task<TagHelperOutput> ProcessTagAsync<TTag>(
        TTag tag, 
        string html,
        string childContent = "") where TTag : TagHelper
    {
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));

        var attributes = new TagHelperAttributeList();
            
        var output = new TagHelperOutput(
            html,
            attributes,
            (result, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetHtmlContent(childContent);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        await tag.ProcessAsync(context, output);

        return output;
    }

    protected async Task<TagHelperOutput> ProcessTagAsync<TTag>(
        TTag tag,
        string html,
        object attributes,
        string childContent = "") where TTag : TagHelper
    {
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));
    
        var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
        var tagHelperAttributes = new TagHelperAttributeList();
    
        dictionary.ForEach(item => tagHelperAttributes.Add(item.Key, item.Value));
            
        var output = new TagHelperOutput(
            html,
            tagHelperAttributes,
            (result, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetHtmlContent(childContent);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
    
        await tag.ProcessAsync(context, output);
    
        return output;
    }

    protected async Task<TagHelperOutput> ProcessTagAsync<TTag>(
        TTag tag, 
        string html, 
        TagHelperAttributeList attributes,
        string childContent = "") where TTag : TagHelper
    {
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            Guid.NewGuid().ToString("N"));
    
        attributes ??= new TagHelperAttributeList();
            
        var output = new TagHelperOutput(
            html,
            attributes,
            (result, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                tagHelperContent.SetHtmlContent(childContent);
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });
    
        await tag.ProcessAsync(context, output);
    
        return output;
    }
        
    protected ModelExpression MakeExpression<TModel>(TModel viewModel, string propertyName, object propertyContent)
    {
        var containerType = viewModel.GetType();

        var m3 = ServiceProvider.GetService<ModelExpressionProvider>();
            
        var compositeMetadataDetailsProvider = ServiceProvider.GetService<ICompositeMetadataDetailsProvider>();
        var metadataProvider = new DefaultModelMetadataProvider(compositeMetadataDetailsProvider);

        var containerMetadata = metadataProvider.GetMetadataForType(containerType);
        var containerExplorer = metadataProvider.GetModelExplorerForType(containerType, viewModel);

        var propertyMetadata = metadataProvider.GetMetadataForProperty(containerType, propertyName);
            
        var modelExplorer = containerExplorer.GetExplorerForExpression(propertyMetadata, propertyContent);

        return new ModelExpression(propertyName, modelExplorer);
    }
        
    public ModelExpression MakeExpression<TModel, TProperty>(
        TModel model, 
        Expression<Func<TModel, TProperty>> expression)
    {
        var modelExpressionProvider = ServiceProvider.GetService<ModelExpressionProvider>();
                
        var compositeMetadataDetailsProvider = ServiceProvider.GetService<ICompositeMetadataDetailsProvider>();
        var metadataProvider = new DefaultModelMetadataProvider(compositeMetadataDetailsProvider);

        var viewDataDictionary = new ViewDataDictionary<TModel>(metadataProvider, new ModelStateDictionary())
        {
            Model = model
        };

        return modelExpressionProvider.CreateModelExpression(viewDataDictionary, expression);
    }
        
    protected ModelExpression MakeExpression<TModel>(TModel model)
    {
        var modelExpressionProvider = ServiceProvider.GetService<ModelExpressionProvider>();
                
        var compositeMetadataDetailsProvider = ServiceProvider.GetService<ICompositeMetadataDetailsProvider>();
        var metadataProvider = new DefaultModelMetadataProvider(compositeMetadataDetailsProvider);

        var viewDataDictionary = new ViewDataDictionary<TModel>(metadataProvider, new ModelStateDictionary())
        {
            Model = model
        };

        var modelExplorer = new ModelExplorer(
            metadataProvider, metadataProvider.GetMetadataForType(model.GetType()), model);

        return new ModelExpression("Model", modelExplorer);
    }
}