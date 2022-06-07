using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Miru.Core;

namespace Miru;

public static class MiruAppExtensions
{
    public static async Task<TResult> ScopedSendAsync<TResult>(
        this IMiruApp app, 
        IRequest<TResult> message,
        CancellationToken ct)
    {
        using var scope = app.WithScope();
        
        var mediator = scope.Get<IMediator>();
                
        try
        {
            return await mediator.Send(message, ct);
        }
        catch (AggregateException e)
        {
            throw e.InnerException ?? e;
        }
    }
    
    public static async Task ScopedPublishAsync<TNotification>(
        this IMiruApp app, 
        TNotification message,
        CancellationToken ct)
    {
        using var scope = app.WithScope();
        
        var mediator = scope.Get<IMediator>();
                
        try
        {
            await mediator.Publish(message, ct);
        }
        catch (AggregateException e)
        {
            throw e.InnerException ?? e;
        }
    }
        
    public static TResult ScopedSendSync<TResult>(
        this IMiruApp app, 
        IRequest<TResult> message,
        CancellationToken ct)
    {
        using var scope = app.WithScope();
            
        var mediator = scope.Get<IMediator>();
                
        try
        {
            return mediator.Send(message, ct).GetAwaiter().GetResult();
        }
        catch (AggregateException e)
        {
            throw e.InnerException ?? e;
        }
    }

    public static ScopedServices WithScope(this IMiruApp app)
    {
        return app.Get<ScopedServices>();
    }
        
    public static void WithScope(this IMiruApp app, Action<ScopedServices> action)
    {
        using var scope = app.Get<ScopedServices>();

        action(scope);
    }
        
    public static TReturn WithScope<TReturn>(this IMiruApp app, Func<ScopedServices, TReturn> func)
    {
        using var scope = app.Get<ScopedServices>();

        return func(scope);
    }
}