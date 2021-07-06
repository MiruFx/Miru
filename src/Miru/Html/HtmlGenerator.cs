using System;
using System.Linq.Expressions;
using HtmlTags;
using HtmlTags.Conventions;
using HtmlTags.Conventions.Elements;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Html
{
    public class FooModel
    {
    }
    
    public class HtmlGenerator
    {
        private readonly IServiceProvider _sp;

        public HtmlGenerator(IServiceProvider sp)
        {
            _sp = sp;
        }
        
        public HtmlTag FormFor<TModel>(TModel model) where TModel : class
        {
            return GeneratorFor(model).TagFor(model, nameof(HtmlConfiguration.Forms));
        }
        
        public HtmlTag FormSummaryFor<TModel>(TModel model) where TModel : class
        {
            return GeneratorFor(model).TagFor(model, nameof(HtmlConfiguration.FormSummaries));
        }
        
        public HtmlTag SubmitFor<TModel>(TModel model) where TModel : class
        {
            return GeneratorFor(model).TagFor(model, nameof(HtmlConfiguration.Submits));
        }

        public HtmlTag LabelFor<T, TProperty>(T model, Expression<Func<T, TProperty>> func) where T : class
        {
            return GeneratorFor(model).LabelFor(func);
        }
        
        public HtmlTag InputFor<T, TProperty>(T model, Expression<Func<T, TProperty>> func) where T : class
        {
            return GeneratorFor(model).InputFor(func);
        }
        
        public HtmlTag SelectFor<T, TProperty>(T model, Expression<Func<T, TProperty>> func) where T : class
        {
            return GeneratorFor(model).TagFor(model, func, nameof(HtmlConfiguration.Selects));
        }
        
        public HtmlTag DisplayFor<T, TProperty>(T model, Expression<Func<T, TProperty>> func) where T : class
        {
            return GeneratorFor(model).DisplayFor(func);
        }
        
        public HtmlTag DisplayLabelFor<T, TProperty>(T model, Expression<Func<T, TProperty>> func) where T : class
        {
            return GeneratorFor(model).TagFor(model, func, nameof(HtmlConfiguration.DisplayLabels));
        }
        
        public HtmlTag CellFor<T, TProperty>(T model, Expression<Func<T, TProperty>> func) where T : class
        {
            var cell = GeneratorFor(model).TagFor(model, func, nameof(HtmlConfiguration.Cells));
            
            cell.Append(GeneratorFor(model).DisplayFor(func, nameof(HtmlConfiguration.Cells)));
            
            return cell;
        }
        
        public HtmlTag TableHeaderFor<T, TProperty>(T model, Expression<Func<T, TProperty>> func) where T : class
        {
            var cell = GeneratorFor(model).TagFor(model, func, nameof(HtmlConfiguration.TableHeader));
            
            cell.Append(DisplayLabelFor(model, func));
            
            return cell;
        }
        
        public IElementGenerator<TModel> GeneratorFor<TModel>(TModel model) where TModel : class
        {
            var htmlConventionLibrary = _sp.GetService<HtmlConventionLibrary>();
            
            return ElementGenerator<TModel>.For(htmlConventionLibrary, t => _sp.GetService(t), model);
        }

        public HtmlTag TagFor(object model, string category)
        {
            var accessor = new OnlyModelAccessor(model);
            
            var request = new ElementRequest(accessor)
            {
                Model = model
            };

            return GeneratorFor(model).TagFor(request, category);
        }
        
        public HtmlTag TagFor(ModelExpression @for, string category)
        {
            var accessor = new ModelMetadataAccessor(@for);
            
            var request = new ElementRequest(accessor)
            {
                Model = @for.Model
            };

            return GeneratorFor(@for.Model).TagFor(request, category);
        }
    }
}