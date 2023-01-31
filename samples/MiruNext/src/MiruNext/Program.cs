using Microsoft.AspNetCore.Hosting;
using MiruNext.Domain;
using MiruNext.Framework;

var builder = WebApplication.CreateBuilder();

builder.Services
    .AddMiruNext<Program, HtmlConfig>();

builder.WebHost.UseUrls("http://localhost:5010");

var app = builder.Build();
app.UseStaticFiles();
app.UseFastEndpoints(cfg => cfg.Endpoints.Configurator = ep =>
{
    ep.AllowAnonymous();
    
    // TODO: source generator
    cfg.Binding.ValueParserFor<OrderStatuses>(x => OrderStatuses.TryFromValue(Convert.ToInt32(x.ToString()), out var status) ? new ParseResult(true, status) : new ParseResult(false, null));
    
    // ep.PostProcessors(EndpointOrder.After, new ObjectResultBehavior());
});
app.Run();