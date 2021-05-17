using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru;
using Miru.Databases.EntityFramework;
using Miru.Domain;
using Miru.Mvc;
using Miru.Security;
using Supportreon.Database;
using Supportreon.Domain;

namespace Supportreon.Features.Users
{
    public class UserEdit : IMustBeAuthenticated
    {
        public class Query : IRequest<Command>
        {
            public long Id { get; set; }
        }

        public class Command : IRequest<Result>
        {
            // lookups
            public Lookups CurrenciesLookup => Currencies.GetAll().ToLookups();
            public Lookups CulturesLookup => Cultures.GetAll().ToLookups();
            public Lookups LanguagesLookup => Languages.GetAll().ToLookups();
                
            //Inputs
            public long Id { get; set; }
            public string Name { get; set; }
            public string Culture { get; set; }
            public string Currency { get; set; }
            public string Language { get; set; }
        }

        public class Result
        {
            public User User { get; set; }
        }

        public class Handler : IRequestHandler<Query, Command>, IRequestHandler<Command, Result>
        {
            private readonly SupportreonDbContext _db;

            public Handler(SupportreonDbContext db)
            {
                _db = db;
            }

            public async Task<Command> Handle(Query request, CancellationToken ct)
            {
                var user = await _db.Users.ByIdOrFailAsync(request.Id, ct);

                return new Command()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Culture = user.Culture,
                    Currency = user.Currency,
                    Language = user.Language
                };
            }

            public async Task<Result> Handle(Command request, CancellationToken ct)
            {
                var user = await _db.Users.ByIdOrFailAsync(request.Id, "User not found", ct);

                user.Name = request.Name;
                user.Culture = request.Culture;
                user.Currency = request.Currency;
                user.Language = request.Language;

                await _db.SaveOrUpdate(user, ct);

                return new Result() {User = user};
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Name).NotEmpty().MaximumLength(256);
            }
        }
        
        public class Cultures : Enumeration<Cultures, string>
        {
            public static Cultures Brazil = new("pt-BR", "Brazil");
            public static Cultures Germany = new("de-DE", "Germany");
            public static Cultures UnitedStates = new("us-US", "United States");

            private Cultures(string value, string name) : base(value, name)
            {
            }
        }

        public class Currencies : Enumeration<Currencies, string>
        {
            public static Currencies BrazilianReal = new("brazilianReal", "Brazilian Real", "R$");
            public static Currencies Euro = new("euro", "Euro", "€");
            public static Currencies Dollar = new("dollar", "Dollar", "$");

            private Currencies(string value, string name, string symbol) : base(value, name)
            {
                Symbol = symbol;
            }

            public string Symbol { get; set; }
        }

        public class Languages : Enumeration<Languages, string>
        {
            public static Languages English = new("en", "English");
            public static Languages German = new("de", "German");
            public static Languages Portuguese = new("pt", "Portuguese");

            private Languages(string value, string name) : base(value, name)
            {
            }
        }

        public class UserController : MiruController
        {
            [HttpGet, Route("/Users/{id:long}/Edit")]
            public async Task<Command> Edit(Query query) => await SendAsync(query);

            [HttpPost, Route("/Users/Save")]
            public async Task<Result> Edit(Command command) => await SendAsync(command);
        }
    }
}