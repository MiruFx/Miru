using System;
using Hangfire.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Miru.Queuing
{
    public class MiruJobActivator : AspNetCoreJobActivator
    {
        private readonly IMediator _mediator;

        public MiruJobActivator(IServiceScopeFactory serviceScopeFactory, IMediator mediator) : base(serviceScopeFactory)
        {
            _mediator = mediator;
        }
        
        public override object ActivateJob(Type type)
        {
            return Activator.CreateInstance(type, _mediator);
        }
    }
}