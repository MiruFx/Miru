using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Miru.Domain;
using Miru.Html;
using Miru.Mvc;

namespace Pantanal.Features.Examples
{
    public class ExampleForm
    {
        public class Query : IRequest<Command>
        {
        }

        public class Command : IRequest<Command>
        {   
            // lookups
            public Lookups CreditCards => CreditCardBrands.GetAll().ToLookups();
            public Lookups Countries { get; set; }

            // inputs
            public int CreditCard { get; set; }
            public Relationships Relationship { get; set; }
            public Address Address { get; set; } = new();
            
            [Checkbox]
            public bool SendInvoice { get; set; }
            
            [Radio]
            public PaymentMethods PaymentMethod { get; set; }
            
            [Radio] 
            public bool Recurrent { get; set; } = true;
            
            [Checkbox]
            public IEnumerable<Newsletters> NewsletterOptions { get; set; }

            public List<ProductItem> Products { get; set; } = new();
        }

        public class Address
        {
            public string Street { get; set; }    
            public string City { get; set; }    
            public string Country { get; set; }    
        }

        public class ProductItem
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
        }
        
        public class Result
        {
        }

        public class Handler : 
            IRequestHandler<Query, Command>,
            IRequestHandler<Command, Command>
        {
            public Task<Command> Handle(Query request, CancellationToken cancellationToken)
            {
                return Task.FromResult(new Command()
                {
                    Countries = new Dictionary<string, string>
                    {
                        { "br", "Brazil" },
                        { "de", "Germany" },
                        { "uk", "United Kingdom" },
                    }.ToLookups(),
                    
                    Products = new List<ProductItem>
                    {
                        new() { Name = "Milk 1L" },
                        new() { Name = "Eggs 10" },
                        new() { Name = "Butter 200g" }
                    },
                    
                    CreditCard = CreditCardBrands.MasterCardBrands.Value,
                    Relationship = Relationships.Married,
                    NewsletterOptions = new[] { Newsletters.Partners },
                    PaymentMethod = PaymentMethods.PayPal,
                    SendInvoice = true
                });
            }
            
            public Task<Command> Handle(Command request, CancellationToken cancellationToken)
            {
                return Task.FromResult(request);
            }
        }

        public class Validation : AbstractValidator<Command>
        {
            public Validation()
            {
                RuleFor(x => x.Address).SetValidator(new AddressValidator());
                
                RuleForEach(x => x.Products).SetValidator(new ProductItemValidator());
            }
        }
        
        public class AddressValidator : AbstractValidator<Address>
        {
            public AddressValidator()
            {
                RuleFor(x => x.Country).NotEmpty();
                RuleFor(x => x.City).NotEmpty();
                RuleFor(x => x.Street).NotEmpty();
            }
        }
        
        public class ProductItemValidator : AbstractValidator<ProductItem>
        {
            public ProductItemValidator()
            {
                RuleFor(x => x.Quantity).NotEmpty().LessThan(5);
            }
        }

        public class Controller : MiruController
        {
            [HttpGet("/Examples/Form")]
            public async Task<Command> Form(Query query) => await SendAsync(query);

            [HttpPost("/Examples/Form")]
            public async Task<Command> Form(Command command) => await SendAsync(command);
        }
        
        public class CreditCardBrands : Enumeration<CreditCardBrands>
        {
            public static CreditCardBrands Visa = new(1, "Visa");
            public static CreditCardBrands MasterCardBrands = new(2, "MasterCard");

            public CreditCardBrands(int value, string name) : base(value, name)
            {
            }
        }

        public enum Relationships
        {
            [Display(Name = "Solteiro")]
            Single = 1,
            
            [Display(Name = "Casado")]
            Married,
            
            [Display(Name = "Divorciado")]
            Divorced
        }
    }

    public enum PaymentMethods
    {
        CreditCard = 1,
        PayPal,
        Bitcoins,
        Blik,
    }
    
    public enum Newsletters
    {
        Offers = 1,
        News,
        Partners
    }
}