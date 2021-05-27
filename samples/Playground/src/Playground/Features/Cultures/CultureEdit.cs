using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Miru.Mvc;

namespace Playground.Features.Cultures
{
    public class CultureEdit
    {
        public class Query : IRequest<Command>
        {
            public string Culture { get; set; }
        }

        public class Command : IRequest<Command>
        {
            public decimal Price { get; set; }
            public string Date { get; set; }
            public string Culture { get; set; }
            
            public decimal DecimalPrice { get; set; }
        }

        public class Handler :
            IRequestHandler<Query, Command>,
            IRequestHandler<Command, Command>
        {
            public Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Command() { Culture = request.Culture });
            }

            public Task<Command> Handle(Command request, CancellationToken cancellationToken)
            {
                // request.DecimalPrice = decimal.Parse(request.Price);
                
                return Task.FromResult(request);
            }
        }

        public class Validation : AbstractValidator<Command>
        {
            public Validation()
            {
                RuleFor(x => x.Price).NotEmpty().GreaterThan(0);

                // RuleFor(x => x.Date).NotEmpty();
            }
            
            protected bool IsADecimal(string s)
            {
                decimal d;

                if(decimal.TryParse(s,out d))
                {
                    return true;
                }
                return false;
            }
        }

        public class Controller : MiruController
        {
            [HttpGet("/Cultures")]
            public async Task<Command> Edit(Query query)
            {
                // TODO: remove browser's language provider otherwise will override cookie
                var localizationOptions = HttpContext.RequestServices.GetRequiredService<IOptions<RequestLocalizationOptions>>();   
                
                Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(query.Culture ?? localizationOptions.Value.DefaultRequestCulture.Culture.Name)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    SameSite = SameSiteMode.Lax
                });
                
                return await SendAsync(query); 
            }

            [HttpPost("/Cultures")]
            public async Task<Command> Edit(Command command) => await SendAsync(command);
        }
    }
}