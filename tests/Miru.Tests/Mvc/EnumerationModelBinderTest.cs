// using System;
// using System.Collections.Generic;
// using System.Globalization;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc.ModelBinding;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.Extensions.DependencyInjection;
// using Miru.Mvc;
//
// namespace Miru.Tests.Mvc;
//
// public class EnumerationModelBinderTest
// {
//     [Test]
//     public async Task Should_bind_enumeration_property()
//     {
//         // var hostBuilder = new HostBuilder()
//         //     .ConfigureWebHost(webHost =>
//         //     {
//         //         // Add TestServer
//         //         webHost.UseTestServer();
//         //         webHost.Configure(app => app.Run(async ctx => 
//         //             await ctx.Response.WriteAsync("Hello World!")));
//         //     });
//         
//         // Build and start the IHost
//         var hostBuilder = TestMiruHost.CreateWebMiruHost<Startup>().ConfigureWebHost(webHost =>
//         {
//             webHost.UseTestServer();
//         });
//             
//         var host = await hostBuilder.StartAsync();
//         
//         // Create an HttpClient to send requests to the TestServer
//         var client = host.GetTestClient();
//         
//         var response = await client.GetAsync("/Orders");
//         
//         response.EnsureSuccessStatusCode();
//         var responseString = await response.Content.ReadAsStringAsync();
//         responseString.ShouldBe("Hello World!");
//             
//         // arrange
//         
//             
//         // var factory = new TestWebApplicationFactory()
//         //     .WithWebHostBuilder(
//         //         builder =>
//         //         {
//         //             // builder
//         //             // builder.ConfigureTestServices(services => {});
//         //         });
//             
//         // act
//         // using var httpClient = factory.CreateClient();
//         //
//         // var response = await httpClient.GetAsync("/Orders");
//         //
//         // Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
//         //
//         // assert
//     }
//     
//     [Test]
//     public async Task Should_not_set_if_value_cannot_be_parsed()
//     {
//         // Arrange
//         var bindingContext = GetBindingContext(typeof(OrderStatus));
//         bindingContext.ValueProvider = new SimpleValueProvider
//         {
//             { "Status", "10" }
//         };
//         var binder = GetBinder();
//     
//         // Act
//         await binder.BindModelAsync(bindingContext);
//     
//         // Assert
//         bindingContext.Result.IsModelSet.ShouldBeFalse();
//     }
//         
//     [Test]
//     public async Task Should_set_if_value_can_be_parsed()
//     {
//         // arrange
//         var bindingContext = GetBindingContext(typeof(OrderStatus));
//         bindingContext.ValueProvider = new SimpleValueProvider
//         {
//             { "Status", OrderStatus.Received.Value.ToString() }
//         };
//         var binder = GetBinder();
//     
//         // act
//         await binder.BindModelAsync(bindingContext);
//     
//         // assert
//         bindingContext.Result.IsModelSet.ShouldBeTrue();
//         bindingContext.Result.Model.ShouldBe(OrderStatus.Received);
//     }
//         
//     [Test]
//     public async Task Should_set_if_value_can_be_parsed_when_enumeration_with_value_type()
//     {
//         // arrange
//         var bindingContext = GetBindingContext(typeof(PaymentStatus));
//         bindingContext.ValueProvider = new SimpleValueProvider
//         {
//             { "Status", PaymentStatus.Refund.Value }
//         };
//         var binder = GetBinder();
//     
//         // act
//         await binder.BindModelAsync(bindingContext);
//     
//         // assert
//         bindingContext.Result.IsModelSet.ShouldBeTrue();
//         bindingContext.Result.Model.ShouldBe(PaymentStatus.Refund);
//     }
//         
//     [Test]
//     public async Task Should_not_set_if_value_cannot_be_parsed_when_enumeration_with_value_type()
//     {
//         // Arrange
//         var bindingContext = GetBindingContext(typeof(PaymentStatus));
//         bindingContext.ValueProvider = new SimpleValueProvider
//         {
//             { "Status", "10" }
//         };
//         var binder = GetBinder();
//     
//         // Act
//         await binder.BindModelAsync(bindingContext);
//     
//         // Assert
//         bindingContext.Result.IsModelSet.ShouldBeFalse();
//     }
//         
//     private IModelBinder GetBinder()
//     {
//         return new SmartEnumModelBinder();
//     }
//     
//     private static DefaultModelBindingContext GetBindingContext(Type modelType = null)
//     {
//         modelType ??= typeof(Ardalis.SmartEnum.SmartEnum<>);
//         return new DefaultModelBindingContext
//         {
//             ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(modelType),
//             ModelName = "Status",
//             ModelState = new ModelStateDictionary(),
//             ValueProvider = new SimpleValueProvider() // empty
//         };
//     }
//         
//     public class TestWebApplicationFactory : WebApplicationFactory<Startup>
//     {
//     }
//         
//     public class Startup
//     {
//         public void ConfigureServices(IServiceCollection services)
//         {
//             services.AddMiru<Startup>(opt => opt.UseEnumerationModelBinding());
//         }
//     
//         public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//         {
//             app.UseRouting();
//     
//             app.UseEndpoints(endpoints =>
//             {
//                 endpoints.MapControllers();
//             });
//         }
//     }
//     
//     public class Query
//     {
//         public OrderStatus OrderStatus { get; set; }
//         public PaymentStatus PaymentStatus { get; set; }
//     }
//     
//     public class Command
//     {
//         public OrderStatus Status { get; set; }
//         public PaymentStatus Payment { get; set; }
//     }
//     
//     public class OrderStatus : Ardalis.SmartEnum.SmartEnum<OrderStatus>
//     {
//         public static OrderStatus Created = new(1, "Created");
//         public static OrderStatus PendingPayment = new(2, "Pending Payment");
//         public static OrderStatus InPreparation = new(3, "In Preparation");
//         public static OrderStatus Shipped = new(4, "Shipped");
//         public static OrderStatus Received = new(5, "Received");
//             
//         public OrderStatus(int value, string name) : base(name, value)
//         {
//         }
//     }
//         
//     public class PaymentStatus : Ardalis.SmartEnum.SmartEnum<PaymentStatus, string>
//     {
//         public static PaymentStatus Pending = new("PE", "Pending");
//         public static PaymentStatus Paid = new("PA", "Pending Payment");
//         public static PaymentStatus Refused = new("NO", "Refund");
//         public static PaymentStatus Refund = new("RE", "Refund");
//     
//         public PaymentStatus(string value, string name) : base(value, name)
//         {
//         }
//     }
//         
//     // public class OrdersController : MiruController
//     // {
//     //     [HttpGet("/Orders")]
//     //     public ContentResult Edit(Query request) => Content(request.Status.Name);
//     //
//     //     [HttpPost("/Orders")]
//     //     public ContentResult Edit(Command request) => Content(request.Status.Name);
//     // }
// }
//     
// public sealed class SimpleValueProvider : Dictionary<string, object>, IValueProvider
// {
//     private readonly CultureInfo _culture;
//     
//     public SimpleValueProvider()
//         : this(null)
//     {
//     }
//     
//     public SimpleValueProvider(CultureInfo culture)
//         : base(StringComparer.OrdinalIgnoreCase)
//     {
//         _culture = culture ?? CultureInfo.InvariantCulture;
//     }
//     
//     public bool ContainsPrefix(string prefix)
//     {
//         foreach (string key in Keys)
//         {
//             if (ModelStateDictionary.StartsWithPrefix(prefix, key))
//             {
//                 return true;
//             }
//         }
//     
//         return false;
//     }
//     
//     public ValueProviderResult GetValue(string key)
//     {
//         if (TryGetValue(key, out var rawValue))
//         {
//             if (rawValue != null && rawValue.GetType().IsArray)
//             {
//                 var array = (Array)rawValue;
//     
//                 var stringValues = new string[array.Length];
//                 for (var i = 0; i < array.Length; i++)
//                 {
//                     stringValues[i] = array.GetValue(i) as string ?? Convert.ToString(array.GetValue(i), _culture);
//                 }
//     
//                 return new ValueProviderResult(stringValues, _culture);
//             }
//             else
//             {
//                 var stringValue = rawValue as string ?? Convert.ToString(rawValue, _culture) ?? string.Empty;
//                 return new ValueProviderResult(stringValue, _culture);
//             }
//         }
//     
//         return ValueProviderResult.None;
//     }
// }