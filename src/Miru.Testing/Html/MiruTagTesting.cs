using System.Linq.Expressions;
using Baseline;
using Baseline.Reflection;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Miru.Html;
using Miru.Html.HtmlConfigs;
using Miru.Html.HtmlConfigs.Core;
using Miru.Html.Tags;
using Miru.Urls;

namespace Miru.Testing.Html;

public abstract class MiruTagTesting : MiruCoreTesting
{
    private static readonly DefaultTagHelperContent TagHelperContent = new();

    private IServiceProvider _sp;
    
    protected TagModifier TagModifier { get; private set; }
    
    protected virtual HtmlConventions HtmlConventions => new DefaultHtmlConfig();
    
    protected virtual void HtmlConfig(HtmlConventions htmlConfig)
    {
    }
    
    public virtual IServiceCollection AddServices(IServiceCollection services) => services;
    
    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        var defaultHtmlConvention = HtmlConventions;
        HtmlConfig(defaultHtmlConvention);
        
        services 
            .AddHtmlConventions(defaultHtmlConvention)
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

        return AddServices(services);
    }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _sp = _.Get<IServiceProvider>();
        TagModifier = _.Get<TagModifier>();
    }

    protected TTag CreateTagWithFor<TTag, TModel>(TTag tag, TModel model) 
        where TTag : MiruForTagHelper, new()
    {
        tag.For = MakeExpression(model);

        return tag;
    }

    public TTag CreateTagWithFor<TTag, TModel, TProperty>(
        TTag tag,
        TModel model, 
        Expression<Func<TModel, TProperty>> expression = null) where TTag : MiruForTagHelper
    {
        tag.For = GetModelExpression(model, expression);

        return tag;
    }
    
    protected TTag Tag<TTag>() where TTag : MiruForTagHelper, new()
    {
        return new TTag 
        { 
            RequestServices = _.App.Get<IServiceProvider>()
        };
    }
    
    protected TTag Tag<TTag>(TTag tag) where TTag : MiruForTagHelper, new()
    {
        tag.RequestServices = _.App.Get<IServiceProvider>();
        return tag;
    }
    
    protected TTag TagWithModel<TTag>(object model) where TTag : MiruForTagHelper, new()
    {
        return new TTag 
        { 
            Model = model, 
            RequestServices = _.App.Get<IServiceProvider>()
        };
    }
    
    protected TTag TagWithXFor<TModel, TProperty, TTag>(
        TTag tag,
        TModel model, 
        Expression<Func<TModel, TProperty>> expression) where TTag : MiruForTagHelper, new()
    {
        return new TTag 
        { 
            RequestServices = _.App.Get<IServiceProvider>(),
            ExFor = ReflectionHelper.GetAccessor(expression),
            ViewContext = new ViewContext
            {
                ViewData = CreateViewData(model)
            }  
        };
    }

    protected TTag TagWithFor<TModel, TProperty, TTag>(
        TTag tag,
        TModel model, 
        Expression<Func<TModel, TProperty>> expression) 
            where TTag : MiruForTagHelper, new()
    {
        tag.For = GetModelExpression(model, expression);
        tag.ViewContext = new ViewContext
        {
            ViewData = CreateViewData(model)
        };
        tag.RequestServices = _.App.Get<IServiceProvider>();
        return tag;
    }
    
    protected async Task<TagHelperOutput> ProcessTagAsync<TTag>(
        TTag tag, 
        string htmlTag,
        string childContent = "") where TTag : TagHelper
    {
        var tagAttributes = new TagHelperAttributeList();
        
        if (childContent == "")
        {
            return new TagHelperOutput(
                htmlTag,
                tagAttributes,
                (_, _) => Task.FromResult<TagHelperContent>(TagHelperContent));
        }
        
        var output = new TagHelperOutput(
            htmlTag,
            tagAttributes,
            (_, _) =>
            {
                TagHelperContent.SetHtmlContent(childContent);
                return Task.FromResult<TagHelperContent>(TagHelperContent);
            });

        var context = new TagHelperContext(
            tagAttributes,
            new Dictionary<object, object>(),
            output.GetHashCode().ToString());
        
        await tag.ProcessAsync(context, output);

        return output;
    }
    
    protected TagHelperOutput ProcessTag<TTag>(
        TTag tagHelper, 
        string htmlTag = "div",
        object attributes = null,
        string content = "") where TTag : TagHelper
    {
        TagHelperOutput output;
        var tagAttributes = new TagHelperAttributeList();
        
        if (attributes != null)
        {
            var dictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(attributes);
            dictionary.ForEach(item => tagAttributes.Add(item.Key, new HtmlString(item.Value.ToString())));
        }
        
        if (content.IsEmpty())
        {
            output = new TagHelperOutput(
                htmlTag,
                tagAttributes,
                (_, _) =>
                {
                    TagHelperContent.SetHtmlContent(string.Empty);
                    return Task.FromResult<TagHelperContent>(TagHelperContent);
                });
        }
        else
        {
            output = new TagHelperOutput(
                htmlTag,
                tagAttributes,
                (_, _) =>
                {
                    TagHelperContent.SetHtmlContent(content);
                    return Task.FromResult<TagHelperContent>(TagHelperContent);
                });
        }

        var context = new TagHelperContext(
            tagAttributes,
            new Dictionary<object, object>(),
            output.GetHashCode().ToString());

        tagHelper.Process(context, output);

        return output;
    }
    
    public ModelExpression GetModelExpression<TModel, TProperty>(
        TModel model, 
        Expression<Func<TModel, TProperty>> expression)
    {
        var modelExpressionProvider = _.Get<ModelExpressionProvider>();
                
        var viewDataDictionary = CreateViewData(model);

        return modelExpressionProvider.CreateModelExpression(viewDataDictionary, expression);
    }
    
    public ModelExpression GetViewContext<TModel, TProperty>(
        TModel model, 
        Expression<Func<TModel, TProperty>> expression)
    {
        var modelExpressionProvider = _.Get<ModelExpressionProvider>();
                
        var viewDataDictionary = CreateViewData(model);

        return modelExpressionProvider.CreateModelExpression(viewDataDictionary, expression);
    }

    public (ModelExpressionProvider, ViewDataDictionary<TModel>) GetModelExpressionProvider<TModel>(TModel model)
    {
        var modelExpressionProvider = _.Get<ModelExpressionProvider>();
                
        var compositeMetadataDetailsProvider = _.Get<ICompositeMetadataDetailsProvider>();
        var metadataProvider = new DefaultModelMetadataProvider(compositeMetadataDetailsProvider);

        var viewDataDictionary = new ViewDataDictionary<TModel>(metadataProvider, new ModelStateDictionary())
        {
            Model = model
        };

        return (modelExpressionProvider, viewDataDictionary);
    }
        
    protected ModelExpression MakeExpression<TModel>(TModel model)
    {
        var modelExpressionProvider = _.Get<ModelExpressionProvider>();
                
        var compositeMetadataDetailsProvider = _.Get<ICompositeMetadataDetailsProvider>();
        var metadataProvider = new DefaultModelMetadataProvider(compositeMetadataDetailsProvider);

        var modelExplorer = new ModelExplorer(
            metadataProvider, 
            metadataProvider.GetMetadataForType(model.GetType()), 
            model);

        return new ModelExpression("Model", modelExplorer);
    }
    
    private ViewDataDictionary<TModel> CreateViewData<TModel>(TModel model)
    {
        var compositeMetadataDetailsProvider = _.Get<ICompositeMetadataDetailsProvider>();
        var metadataProvider = new DefaultModelMetadataProvider(compositeMetadataDetailsProvider);

        var viewDataDictionary = new ViewDataDictionary<TModel>(metadataProvider, new ModelStateDictionary())
        {
            Model = model
        };
        
        return viewDataDictionary;
    }
}