using Bogus;

namespace Miru.Testing
{
    public static class FakerExtensions
    {
        public static Faker<T> With<T>(this Faker<T> faker, Action<T> action) where T : class
        {
            return faker.FinishWith((f, m) => action(m));
        }
        
        public static T GenerateWith<T>(this Faker<T> faker, Action<T> action) where T : class
        {
            T entity = faker.Generate();

            action(entity);

            return entity;
        }
        
        public static IEnumerable<T> GenerateWith<T>(this Faker<T> faker, int count, Action<T> action) where T : class
        {
            var entities = faker.Generate(count);

            foreach (var entity in entities)
            {
                action(entity);    
            }

            return entities;
        }
    }
}