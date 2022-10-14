// using System;
// using System.Linq;
// using AV.Enumeration;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
// using Miru.Domain;
//
// namespace Miru.Databases.EntityFramework
// {
//     public static class ModelBuilderExtensions
//     {
//         public static ModelBuilder UseValueConverterForType<T>(this ModelBuilder modelBuilder, ValueConverter converter)
//         {
//             return modelBuilder.UseValueConverterForType(typeof(T), converter);
//         }
//
//         public static ModelBuilder UseValueConverterForType(this ModelBuilder modelBuilder, Type type, ValueConverter converter)
//         {
//             foreach (var entityType in modelBuilder.Model.GetEntityTypes())
//             {
//                 // note that entityType.GetProperties() will throw an exception, so we have to use reflection 
//                 var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == type);
//                 
//                 foreach (var property in properties)
//                 {
//                     modelBuilder
//                         .Entity(entityType.Name)
//                         .Property(property.Name)
//                         .HasConversion(converter);
//                 }
//             }
//
//             return modelBuilder;
//         }
//         
//         public static ModelBuilder PropertiesEnumeration<TEnumeration>(this ModelBuilder modelBuilder) 
//             where TEnumeration : Enumeration
//         {
//             foreach (var entityType in modelBuilder.Model.GetEntityTypes())
//             {
//                 var properties = entityType.ClrType
//                     .GetProperties()
//                     .Where(p => p.PropertyType.Implements(typeof(TEnumeration)));
//                 
//                 foreach (var property in properties)
//                 {
//                     modelBuilder
//                         .Entity(entityType.Name)
//                         .Property<TEnumeration>(property.Name)
//                         .HasConversion(v => (int?) v.Value, v => v.HasValue ? Enumeration.FromValue<TEnumeration>(v.Value) : null);
//                 }
//             }
//
//             return modelBuilder;
//         }
//         
//         public static ModelBuilder PropertiesImplementing<TType, TProperty>(
//             this ModelBuilder modelBuilder, 
//             Action<PropertyBuilder<TProperty>> action)
//         {
//             foreach (var entityType in modelBuilder.Model.GetEntityTypes())
//             {
//                 // note that entityType.GetProperties() will throw an exception, so we have to use reflection 
//                 var properties = entityType.ClrType
//                     .GetProperties()
//                     .Where(p => p.PropertyType.Implements(typeof(TType)));
//                 
//                 foreach (var property in properties)
//                 {
//                     var propertyBuilder = modelBuilder
//                         .Entity(entityType.Name)
//                         .Property<TProperty>(property.Name);
//                         
//                     action(propertyBuilder);
//                 }
//             }
//
//             return modelBuilder;
//         }
//     }
// }