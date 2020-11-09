namespace Miru.Domain
{
    public static class EntityExtensions
    {
        public static bool IsNew(this IEntity entity)
        {
            return entity.Id.Equals(default(long)) || entity.Id < 0; 
        }
    }
}