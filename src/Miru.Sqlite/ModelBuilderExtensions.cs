using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Miru.Databases.EntityFramework;

namespace Miru.Sqlite
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder UseSqlite(this ModelBuilder builder)
        {
            // Makes SQLite support decimal order by
            return builder.UseValueConverterForType<decimal>(new CastingConverter<decimal, double>());
        }
    }
}