using System.ComponentModel.DataAnnotations;
using Miru.Domain;
using Miru.Html;

namespace Corpo.Skeleton.Features.Examples;

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
            
        [Checkbox]
        public IEnumerable<Newsletters> NewsletterOptions { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }    
        public string City { get; set; }    
        public string Country { get; set; }    
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
    
// public class EnumerationBinderProvider : IModelBinderProvider
// {
//     public IModelBinder GetBinder(ModelBinderProviderContext context)
//     {
//         if (context == null)
//         {
//             throw new ArgumentNullException(nameof(context));
//         }
//
//         if (context.Metadata.ModelType == typeof(Enumeration<,>))
//         {
//             return new BinderTypeModelBinder(typeof(EnumerationBinder));
//         }
//
//         return null;
//     }
// }
//
// public class EnumerationBinder : IModelBinder
// {
//     public Task BindModelAsync(ModelBindingContext bindingContext)
//     {
//         if (bindingContext == null)
//         {
//             throw new ArgumentNullException(nameof(bindingContext));
//         }
//
//         var modelName = bindingContext.ModelName;
//
//         // Try to fetch the value of the argument by name
//         var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
//
//         if (valueProviderResult == ValueProviderResult.None)
//         {
//             return Task.CompletedTask;
//         }
//
//         bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);
//
//         var value = valueProviderResult.FirstValue;
//
//         // Check if the argument value is null or empty
//         if (string.IsNullOrEmpty(value))
//         {
//             return Task.CompletedTask;
//         }
//
//         var item = bindingContext.
//         if (!int.TryParse(value, out var id))
//         {
//             // Non-integer arguments result in model state errors
//             bindingContext.ModelState.TryAddModelError(modelName, "Author Id must be an integer.");
//
//             return Task.CompletedTask;
//         }
//
//         // Model will be null if not found, including for
//         // out of range id values (0, -3, etc.)
//         var model = _context.Authors.Find(id);
//         bindingContext.Result = ModelBindingResult.Success(model);
//         return Task.CompletedTask;
//     }
// }