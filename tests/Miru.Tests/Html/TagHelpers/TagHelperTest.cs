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
using Miru.Testing;
using Miru.Urls;
using NUnit.Framework;

namespace Miru.Tests.Html.TagHelpers
{
    public class TagHelperTest
    {
        protected IServiceProvider ServiceProvider { get; set; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var services = new ServiceCollection()
                .AddMiruHtml(new HtmlConfiguration().AddTwitterBootstrap())
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
        
        protected TagHelperOutput ProcessTag<TTag>(
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

            tag.Process(context, output);

            return output;
        }
        
        protected TagHelperOutput ProcessTag<TTag>(
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

            tag.Process(context, output);

            return output;
        }
        
        protected async Task<TagHelperOutput> ProcessTagAsync<TTag>(TTag tag, string tagName, string childContent = "") where TTag : TagHelper
        {
            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));
            
            var output = new TagHelperOutput(
                tagName,
                new TagHelperAttributeList(),
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
        
        protected ModelExpression MakeExpression<TModel, TProperty>(
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
}