using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Miru.Html.HtmlConfigs.Core;

public interface IConventionPipeline
{
    void Build(TagHelperOutput output);
}

public class ConventionPipeline : IConventionPipeline
{
    private readonly ModelExpression _modelExpression;
    private readonly IList<Builder> _builders;

    public ConventionPipeline(ModelExpression requestData, IList<Builder> builders)
    {
        _modelExpression = requestData;
        _builders = builders;
    }

    public void Build(TagHelperOutput output)
    {
        for (var index = 0; index < _builders.Count; index++)
        {
            var builder = _builders[index];
            
            if (builder.Condition(_modelExpression))
            {
                var pipeLineFunc = builder.BuilderFuncPipeline;

                if (pipeLineFunc != null)
                    pipeLineFunc(_modelExpression, this, output);
                else
                    builder.BuilderFunc(_modelExpression, output);
            }
        }
    }
}