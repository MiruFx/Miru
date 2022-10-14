using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Miru.Sqlite;

public static class ModelBuilderExtensions
{
    public static ModelBuilder UseSqlite(this ModelBuilder builder)
    {
        // Makes SQLite support decimal order by
        return builder.UseValueConverterForType<decimal>(new CastingConverter<decimal, double>());
    }
    
    public static ModelBuilder UseValueConverterForType<T>(this ModelBuilder modelBuilder, ValueConverter converter)
    {
        return modelBuilder.UseValueConverterForType(typeof(T), converter);
    }

    public static ModelBuilder UseValueConverterForType(
        this ModelBuilder modelBuilder, 
        Type type,
        ValueConverter converter)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // note that entityType.GetProperties() will throw an exception, so we have to use reflection
            var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == type);

            foreach (var property in properties)
            {
                modelBuilder.Entity(entityType.Name).Property(property.Name)
                    .HasConversion(converter);
            }
        }

        return modelBuilder;
    }
}
