using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Pipeline;

public class PipelineBuilder
{
    private readonly IServiceCollection _services;

    public PipelineBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public void UseBehavior(Type behaviorType)
    {
        _services.AddScoped(typeof(IPipelineBehavior<,>), behaviorType);
    }
}